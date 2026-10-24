# Part 0

## 0.1–0.3: HTML, CSS, and forms

Reviewed the assigned MDN introductions to HTML structure, CSS rules and the box model, and form controls and submission.

## 0.4: Creating a note

Paths below are relative to `https://studies.cs.helsinki.fi/exampleapp`.

```mermaid
sequenceDiagram
    participant B as Browser
    participant S as Server
    Note over B: Enter note and select Save
    B->>S: POST /new_note (form data)
    Note over S: Append note with server timestamp
    S-->>B: 302, Location /exampleapp/notes
    B->>S: GET /notes
    S-->>B: HTML
    B->>S: GET /main.css
    S-->>B: CSS
    B->>S: GET /main.js
    S-->>B: JavaScript
    Note over B: Execute script
    B->>S: GET /data.json
    S-->>B: Notes JSON
    Note over B: Callback renders updated list
```

## 0.5: Opening the single-page app

```mermaid
sequenceDiagram
    participant B as Browser
    participant S as Server
    B->>S: GET /exampleapp/spa
    S-->>B: HTML
    B->>S: GET /exampleapp/main.css
    S-->>B: CSS
    B->>S: GET /exampleapp/spa.js
    S-->>B: JavaScript
    Note over B: Execute script and register form handler
    B->>S: GET /exampleapp/data.json
    S-->>B: Notes JSON
    Note over B: Store notes and render list
```

CSS and script requests may overlap; their order does not affect the distinction between loading the document and fetching notes.

## 0.6: Creating a note in the single-page app

```mermaid
sequenceDiagram
    participant B as Browser
    participant S as Server
    Note over B: Submit handler prevents default navigation
    Note over B: Create note with content and timestamp
    Note over B: Append locally, clear input, redraw list
    B->>S: POST /exampleapp/new_note_spa, application/json
    Note over S: Append received note
    S-->>B: 201 Created
    Note over B: Remain on current page without reload
```
