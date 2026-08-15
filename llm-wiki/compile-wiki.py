import json
import pathlib
import urllib.request


MODEL = "qwen2.5-coder:7b"
OLLAMA_URL = "http://localhost:11434/api/chat"

BASE_DIR = pathlib.Path(__file__).parent
INPUT_DIR = BASE_DIR / "input"
WIKI_DIR = BASE_DIR / "wiki"


def read_sources():
    sources = []

    for file in sorted(INPUT_DIR.glob("*.md")):
        content = file.read_text(encoding="utf-8")

        sources.append({
            "file": file.name,
            "content": content
        })

    return sources


def build_prompt(sources):
    documents = []

    for source in sources:
        documents.append(
            f"""
===== SOURCE: {source["file"]} =====

{source["content"]}

===== END SOURCE =====
"""
        )

    return f"""
You are a knowledge compiler.

Your job is to compile the provided software architecture
documents into a structured knowledge base.

IMPORTANT RULES:

1. Never invent facts.
2. Every fact MUST have at least one source file.
3. If something is not specified by the sources, say that it
   is not specified.
4. Do not infer Product attributes, infrastructure details,
   security requirements, scalability characteristics, etc.
   unless explicitly mentioned in the sources.
5. Preserve relationships between concepts and components.
6. Detect contradictions between sources.
7. Prefer facts from the sources over your own general knowledge.

Return ONLY valid JSON.

Required structure:

{{
  "pages": [
    {{
      "title": "Page title",
      "slug": "page-slug",
      "summary": "Short summary based only on sources",
      "facts": [
        {{
          "text": "A factual statement",
          "sources": ["source-file.md"]
        }}
      ],
      "relationships": [
        {{
          "from": "Entity A",
          "relation": "uses",
          "to": "Entity B",
          "sources": ["source-file.md"]
        }}
      ],
      "unknowns": [
        "Something that is not specified in the sources"
      ]
    }}
  ]
}}

SOURCE DOCUMENTS:

{''.join(documents)}
"""


def call_ollama(prompt):
    payload = {
        "model": MODEL,
        "messages": [
            {
                "role": "user",
                "content": prompt
            }
        ],
        "stream": False,
        "format": "json"
    }

    data = json.dumps(payload).encode("utf-8")

    request = urllib.request.Request(
        OLLAMA_URL,
        data=data,
        headers={
            "Content-Type": "application/json"
        }
    )

    with urllib.request.urlopen(request) as response:
        result = json.loads(response.read())

    return json.loads(result["message"]["content"])


def write_wiki(result):
    WIKI_DIR.mkdir(exist_ok=True)

    for page in result["pages"]:
        file = WIKI_DIR / f'{page["slug"]}.md'

        lines = [
            f'# {page["title"]}',
            "",
            page["summary"],
            "",
            "## Facts",
            ""
        ]

        for fact in page["facts"]:
            sources = ", ".join(
                f"`{source}`"
                for source in fact["sources"]
            )

            lines.append(
                f'- {fact["text"]}  '
                f'\n  Source: {sources}'
            )

        lines.extend([
            "",
            "## Relationships",
            ""
        ])

        for relationship in page["relationships"]:
            sources = ", ".join(
                f"`{source}`"
                for source in relationship["sources"]
            )

            lines.append(
                f'- **{relationship["from"]}** '
                f'→ **{relationship["relation"]}** '
                f'→ **{relationship["to"]}**  '
                f'\n  Source: {sources}'
            )

        lines.extend([
            "",
            "## Unknown / Not Specified",
            ""
        ])

        for unknown in page["unknowns"]:
            lines.append(f"- {unknown}")

        file.write_text(
            "\n".join(lines),
            encoding="utf-8"
        )


def main():
    sources = read_sources()

    if not sources:
        print("No Markdown files found in input/")
        return

    print(f"Found {len(sources)} source documents.")

    prompt = build_prompt(sources)

    print("Sending documents to Ollama...")
    result = call_ollama(prompt)

    print(f"Generated {len(result['pages'])} wiki pages.")

    write_wiki(result)

    print("Wiki generated successfully.")
    print(f"Output: {WIKI_DIR}")


if __name__ == "__main__":
    main()