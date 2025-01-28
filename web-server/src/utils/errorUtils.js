export const getErrorMessage = async (response) => {
    try {
        const clonedResponse = response.clone(); // Clona la risposta
        const errorData = await clonedResponse.json();
        const errorDetails = errorData.errors;

        // Formattazione dei messaggi di errore
        return Object.entries(errorDetails)
            .map(([key, messages]) => `${key}: ${messages.join(', ')}`)
            .join('\n');
    } catch {
        // Fallback al plain text
        return await response.text();
    }
};