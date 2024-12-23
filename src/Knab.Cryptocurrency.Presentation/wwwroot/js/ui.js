export function showLoading() {
    document.getElementById("loading").style.display = "block";
}

export function hideLoading() {
    document.getElementById("loading").style.display = "none";
}

const showPriceData = (pairPrice, lastUpdated) => {
    return `
          <div class="bg-gray-700/50 rounded-lg p-6 mt-3">
            <div>
              <div class="text-3xl font-bold">
                ${pairPrice}
              </div>
            </div>
            <div class="text-xs text-gray-400 text-right">
              ${lastUpdated}
            </div>
          </div>
  `;
};

export function showError(message) {
    const priceContainer = document.getElementById("price-container");
    priceContainer.innerHTML = `
    <div class="bg-red-900/50 border border-red-700 rounded-lg p-4 text-red-200" id="error-container">
      ${message}
    </div>
  `;
}

export function updatePriceDisplay(quotes) {
    debugger;
    const container = document.getElementById("price-container");
    const fiats = quotes.map((quote) => {
        return showPriceData(quote.exchangeRate + " " + quote.quoteCurrency.code, formatToReadableDate(quote.timestamp));
    });

    container.innerHTML = fiats.join("");
}

function formatToReadableDate(inputDate) {
    const date = new Date(inputDate);

    const formattedDate = `${date.getUTCFullYear()}-${String(date.getUTCMonth() + 1).padStart(2, '0')}-${String(date.getUTCDate()).padStart(2, '0')} ${String(date.getUTCHours()).padStart(2, '0')}:${String(date.getUTCMinutes()).padStart(2, '0')}:${String(date.getUTCSeconds()).padStart(2, '0')}`;

    return formattedDate;
}
