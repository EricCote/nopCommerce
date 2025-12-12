<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Accordion Component</title>
  <style>
    /* 1. Theme Variables */
    :root {
      --bg-card: #ffffff;
      --border-color: #e5e7eb;  /* gray-200 */
      --primary-color: #3b82f6; /* blue-500 */
      --text-gray-900: #111827;
      --text-gray-700: #374151;
      --font-family: ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
    }

    body {
      font-family: var(--font-family);
      padding: 2rem;
      background-color: #f3f4f6; /* Light gray background to show card contrast */
    }

    /* 2. Component Styles */

    /* Container (details) */
    .accordion-item {
      background-color: var(--bg-card);
      border-radius: 0.5rem;      /* rounded-lg */
      border: 2px solid var(--border-color); /* border-2 border-border */
      overflow: hidden;
      transition: border-color 150ms ease;   /* transition on hover */
      max-width: 600px; /* Optional: limit width for demo */
    }

    .accordion-item:hover {
      border-color: var(--primary-color); /* hover:border-primary */
    }

    /* Trigger (summary) */
    .accordion-trigger {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 2rem; /* p-8 */
      cursor: pointer;
      list-style: none; /* list-none */
      outline: none;
    }

    /* Hide default triangle marker (Chrome/Safari/Edge) */
    .accordion-trigger::-webkit-details-marker {
      display: none;
    }

    /* Title (h3) */
    .accordion-title {
      font-size: 1.5rem; /* text-2xl */
      line-height: 2rem;
      font-weight: 700;  /* font-bold */
      color: var(--text-gray-900);
      margin: 0;
    }

    /* Icon (svg) */
    .accordion-icon {
      width: 1.5rem;
      height: 1.5rem;
      color: var(--primary-color);
      transition: transform 300ms ease;
    }

    /* Icon Rotation Logic */
    .accordion-item[open] .accordion-icon {
      transform: rotate(180deg);
    }

    /* Content Wrapper */
    .accordion-content {
      padding: 1rem 2rem 2rem 2rem; /* pt-4 px-8 pb-8 */
      border-top: 1px solid var(--border-color); /* border-t */
    }

    /* Spacing between steps (space-y-6) */
    .accordion-step-container > * + * {
      margin-top: 1.5rem; 
    }

    /* Step Typography */
    .step-title {
      font-size: 1.125rem; /* text-lg */
      font-weight: 700;
      color: var(--text-gray-900);
      margin-bottom: 0.75rem; /* mb-3 */
      margin-top: 0;
    }

    .step-text {
      color: var(--text-gray-700);
      margin-bottom: 0.75rem; /* mb-3 */
      line-height: 1.5;
      margin-top: 0;
    }

    /* Lists */
    .step-list {
      list-style-type: disc;
      list-style-position: inside;
      color: var(--text-gray-700);
      margin-left: 1rem; /* ml-4 */
      padding: 0;
    }

    /* List item spacing (space-y-2) */
    .step-list li + li {
      margin-top: 0.5rem;
    }
  </style>
</head>
<body>

  <details class="accordion-item">
    <summary class="accordion-trigger">
      <h3 class="accordion-title">How to Install</h3>
      <svg 
        xmlns="http://www.w3.org/2000/svg" 
        width="24" 
        height="24" 
        viewBox="0 0 24 24" 
        fill="none" 
        stroke="currentColor" 
        stroke-width="2" 
        stroke-linecap="round" 
        stroke-linejoin="round" 
        class="accordion-icon" 
        aria-hidden="true"
      >
        <path d="m6 9 6 6 6-6"></path>
      </svg>
    </summary>

    <div class="accordion-content">
      <div class="accordion-step-container">
        
        <div>
          <h4 class="step-title">Step 1: Prerequisites</h4>
          <p class="step-text">
            Before you begin, make sure you have the following installed on your system:
          </p>
          <ul class="step-list">
            <li>Node.js (version 16 or higher)</li>
            <li>npm or yarn package manager</li>
            <li>A modern web browser</li>
          </ul>
        </div>

      </div>
    </div>
  </details>

</body>
</html>