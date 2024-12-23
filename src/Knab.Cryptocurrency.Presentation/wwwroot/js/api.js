export async function fetchQuotes(name, slug) {
    const response = await fetch(`/api/Quotes/${name}/${slug}`);

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.Exception);
    }

    return await response.json();
}

export async function fetchCryptoCurrencies() {
    const response = await fetch("/api/CryptoCurrency");

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.Exception);
    }

    return await response.json();
}
