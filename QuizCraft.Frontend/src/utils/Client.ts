import { BackendUri } from "./Environment";
import { Client } from "./QuizCraftAPIClient";

const httpClient = {
    fetch: (url: RequestInfo, init?: RequestInit): Promise<Response> => {
        const token = sessionStorage.getItem("token");
        
        const headers = new Headers(init?.headers || {});

        if (token)
            headers.set("Authorization", `Bearer ${token}`);
        
        return fetch(url, { ...init, headers });
    }
};

export const client = new Client(BackendUri, httpClient);