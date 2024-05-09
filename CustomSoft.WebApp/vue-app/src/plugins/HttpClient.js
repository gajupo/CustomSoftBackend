export default class HttpClient {
    constructor(baseURL) {
        this.baseURL = baseURL;
    }

    async get(path) {
        try {
            const response = await fetch(`${this.baseURL}${path}`);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return await response.json();
        } catch (error) {
            console.error("HTTP GET Request Failed: ", error);
            throw error;
        }
    }

    async getAsBlob(path) {
        try {
            const response = await fetch(`${this.baseURL}${path}`);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return await response.blob();
        } catch (error) {
            console.error("HTTP GET Request Failed: ", error);
            throw error;
        }
    }

    async post(path, data, json = true) {
        try {
            const headers = json ? { 'Content-Type': 'application/json' } : {};
            const body = json ? JSON.stringify(data) : data;

            const response = await fetch(`${this.baseURL}${path}`, {
                method: 'POST',
                headers: headers,
                body: body
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return await response.json();
        } catch (error) {
            console.error("HTTP POST Request Failed: ", error);
            throw error;
        }
    }

    async postFiles(path, files, additionalData = {}) {
        const formData = new FormData();

        for (const element of files) {
            formData.append('files', element);
        }

        // Append additional data if needed
        Object.keys(additionalData).forEach(key => formData.append(key, additionalData[key]));
        try {
            const response = await fetch(`${this.baseURL}${path}`, {
                method: 'POST',
                body: formData
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return true;
        } catch (error) {
            console.error("HTTP File Upload POST Request Failed: ", error);
            throw error;
        }
    }

    async put(path, data) {
        try {
            const response = await fetch(`${this.baseURL}${path}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return true;
        } catch (error) {
            console.error("HTTP PUT Request Failed: ", error);
            throw error;
        }
    }

    async delete(path) {
        try {
            const response = await fetch(`${this.baseURL}${path}`, {
                method: 'DELETE'
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return true;
        } catch (error) {
            console.error("HTTP DELETE Request Failed: ", error);
            throw error;
        }
    }
}
