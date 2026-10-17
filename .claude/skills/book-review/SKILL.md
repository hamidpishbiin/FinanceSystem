---
name: book-review
description: Review the current uncommitted changes (or a given path/commit range) against the book checklists in docs/review-rules/ (Evans and Vernon for DDD, Geewax for API design). Use when Hamid asks to "review against the book(s)", "book review", or "/book-review".
---

# Book review

Review code changes against the distilled book checklists in `docs/review-rules/`.

## Sources

- `docs/review-rules/evans-ddd.md` — Evans, *Domain-Driven Design*: core concepts and reasoning. Rule IDs `EV-…`.
- `docs/review-rules/vernon-iddd.md` — Vernon, *Implementing Domain-Driven Design*: practical implementation. Rule IDs `VN-…`.
- `docs/review-rules/geewax-api-design-patterns.md` — Geewax, *API Design Patterns*: the API surface (routes, verbs, status codes, request/response models, ids, errors, idempotency, retries). Rule IDs `AP-…`. Use it for controllers, API models, facades and gateway/callback code. PDF page = printed page + 36.

When several books cover the same defect, report it once and cite all the IDs.

If a finding hinges on detail the checklist doesn't cover, the original PDFs are in `/Users/hamid/Dev/Books/`. Read only the relevant pages (`pdftotext -f N -l M`), never the whole book.

## Scope

- No argument: all uncommitted changes — `git diff HEAD` plus untracked files from `git status --short`.
- A path: changes under that path only.
- A commit range (e.g. `HEAD~3..HEAD`): that range.

Read every changed file in full, not just the hunks — aggregate and layering rules depend on context.

## Procedure

1. Read the checklist file(s) for the changed file kinds in full (all three when the diff touches both domain and API code), starting with the **Project decisions that override the book** section. Also respect `CLAUDE.local.md` and memory (Product types hands-off, no code comments, minimal first, repositories in Domain).
2. For each changed file, pick the relevant sections (the checklist's last section maps file kinds to sections).
3. Check each applicable rule against the code. Verify before reporting: open the referenced types, check callers, build if a claim depends on compilation.
4. Report, most severe first:

   ```
   [EV-AGG-2] Breaks — code/Domain/.../Bill.cs:12
   What: <the defect in one sentence>
   Book: <one sentence of the book's reasoning, cite the rule ID>
   Suggest: <concrete change>
   ```

   Severities: **Breaks** / **Smell** / **Learning note** (defined in the checklist).
5. End with a one-line verdict and, separately, any place where the book and a recorded project decision disagree (mention once, not as a defect).

## Rules for the reviewer

- Report only; don't edit code unless asked.
- Prefer a few meaningful findings over many nitpicks; skip rules the diff doesn't touch.
- Never invent page numbers or rules — cite only IDs present in the checklist files.
- Explain the pattern and the trade-off (Hamid is learning DDD), but keep each finding short.
