import { Client } from "./QuizCraftAPIClient";

const httpClient = {
    fetch: (url: RequestInfo, init?: RequestInit): Promise<Response> => {
        return fetch(url, init);
    }
};

export const client = new Client("https://localhost:8080", httpClient);