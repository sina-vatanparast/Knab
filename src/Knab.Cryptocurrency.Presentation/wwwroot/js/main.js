import { fetchQuotes, fetchCryptoCurrencies } from "./api.js";
import { showLoading, hideLoading, updatePriceDisplay, showError, } from "./ui.js";
async function updatePrice(name, slug) {
    showLoading();
    try {
        const quotes = await fetchQuotes(name, slug);
        updatePriceDisplay(quotes);
    } catch (err) {
        showError(err.message);
    } finally {
        hideLoading();
    }
}

async function fillCryptoCurrencies(cryptoSelect) {
    try {
        const cryptocurrencies = await fetchCryptoCurrencies();
        cryptoSelect.innerHTML = '';
        cryptocurrencies.forEach(crypto => {
            const option = document.createElement('option');
            option.value = crypto.slug; 
            option.textContent = crypto.name;
            cryptoSelect.appendChild(option);
        });
        updatePrice(cryptoSelect.options[cryptoSelect.selectedIndex].textContent, cryptoSelect.value);
    } catch (err) {
        alert(err.message);
    } 
}

document.addEventListener("DOMContentLoaded", () => {
    const cryptoSelect = document.getElementById("crypto-select");
    const refreshButton = document.getElementById("refresh-button");

    fillCryptoCurrencies(cryptoSelect);

    cryptoSelect.addEventListener("change", (e) => updatePrice(cryptoSelect.options[cryptoSelect.selectedIndex].textContent, cryptoSelect.value));
    refreshButton.addEventListener("click", () => updatePrice(cryptoSelect.options[cryptoSelect.selectedIndex].textContent, cryptoSelect.value));
});
