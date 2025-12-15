

const emojiList =`🤨🧐😒😞😔😟😕🙁😣😖😫😩🥺😢😭😤😠😡🤬🤯😳🥵🥶😱😨😰😥😓🤔😶😐😑😬🙄😯😦😧😮😲🥱😪😵🤐🥴🤢🤮🤒🤕👿👹👺💩💀🙀😿😾🧟🙇🙅🤦🤷🙎💣🧨💔⛔📛`

function getRandomEmoji() {
  const emojis = Array.from(emojiList);
  
  const randomIndex = Math.floor(Math.random() * emojis.length);
  return emojis[randomIndex];
}

/**
 * Displays a simple, dismissible error popup with the given text.
 * The popup can be dismissed by clicking the 'Dismiss' button or by 
 * clicking anywhere outside the error box (on the overlay).
 *
 * @param {string} text - The error message to display inside the popup.
 */
function myPopupError(text, btnText, messageType, timeout, callback) {
  // 1. Check if an existing overlay is present and remove it 
  // to prevent multiple popups from stacking.
  const existingOverlay = document.getElementById('popup-error-overlay');
  if (existingOverlay) {
    existingOverlay.remove();
  }

  // 2. Create the main overlay element
  const overlay = document.createElement('div');
  overlay.id = 'popup-error-overlay';
  // Style the overlay to cover the entire screen and provide a backdrop
  overlay.style.cssText = `
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.5); /* Semi-transparent black backdrop */
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 1000; /* Ensure it's on top of other content */
    `;

  // Function to dismiss and clean up the popup
  const dismissPopup = () => {
    overlay.remove();
    callback && callback();
  };

  // 3. Create the error box element
  const errorBox = document.createElement('div');
  errorBox.id = 'popup-error-box';
  // Style the actual popup container
  errorBox.style.cssText = `
        background-color: white;
        padding: 20px;
        padding-top: 50px;
        border-radius: 8px;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.7);
        max-width: 400px;
        width: 90%;
        text-align: center;
        border: 2px solid #3b0707ff; /* Red border for error visual */
        position: relative;
    `;


  // --- NEW ICON IMPLEMENTATION ---
  // 3.5 Create the icon element
  const errorIcon = document.createElement('div');
  errorIcon.innerHTML = getRandomEmoji();
  errorIcon.style.cssText = `
        background-color: #992222; /* Bright red background */ 
        color: #ffffff; /* Bright white color */
        width: 80px;
        height: 80px;
        border-radius: 100%;
        font-size: 50px; /* Large size */
        line-height: 1.0;
        display: flex;
        flex-wrap: wrap;
        align-items: center;
        justify-content: center;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.8);
        position: absolute;
        top: -40px;
        left: 50%;
        transform: translateX(-50%);
    `;

  // 4. Create the error text element
  const errorText = document.createElement('p');
  errorText.textContent = `${text}`;
  errorText.style.cssText = `
        font-weight: bold;
        margin-bottom: 15px;
        font-size: 1.1em;
        font-family: sans-serif;
    `;

  // 5. Create the dismiss button
  const dismissButton = document.createElement('button');
  dismissButton.textContent = btnText;
  // Style the button
  dismissButton.style.cssText = `
        background-color: #992222; /* Bright red button */
        color: white;
        border: none;
        padding: 10px 20px;
        border-radius: 4px;
        cursor: pointer;
        font-size: 1em;
        transition: background-color 0.2s;
    `;

  // Add hover effect via JavaScript for better compatibility
  dismissButton.onmouseover = function () {
    this.style.backgroundColor = '#771111'; // Darken on hover
  };
  dismissButton.onmouseout = function () {
    this.style.backgroundColor = '#992222'; // Revert on mouseout
  };

  // 6. Attach the dismiss function to the button click
  dismissButton.addEventListener('click', dismissPopup);

  // 7. Assemble the components
  errorBox.appendChild(errorIcon);
  errorBox.appendChild(errorText);
  errorBox.appendChild(dismissButton);
  overlay.appendChild(errorBox);

  // 8. Add event listener to dismiss popup when clicking the overlay (outside the box)
  overlay.addEventListener('click', (event) => {
    // Only dismiss if the click happened directly on the overlay, not on the errorBox
    if (event.target === overlay) {
      dismissPopup();
    }
  });

  // 9. Append the final overlay to the document body
  document.body.appendChild(overlay);
}