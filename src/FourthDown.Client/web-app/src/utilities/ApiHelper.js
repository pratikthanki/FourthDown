const base_url = "/api";

const tryParseResponse = async (response) => {
    const rawData = await response.text();
    try {
        return JSON.parse(rawData);
    } catch {
        return { title: "Request error", errors: { text: [rawData] } };
    }
};

const getRequest = async (endpoint) => {
    const response = await fetch(`${base_url}/${endpoint}`);
    const data = await tryParseResponse(response);
    return [response.status === 200, data];
};

export const getTeams = async () => {
    return getRequest('teams');
};

