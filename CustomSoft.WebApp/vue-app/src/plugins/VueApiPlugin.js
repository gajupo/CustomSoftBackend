import HttpClient from './HttpClient'; 

const ApiPlugin = {
    install(app) {
        const httpClient = new HttpClient("https://localhost:7260");
        app.config.globalProperties.$api = httpClient;
    }
};

export default ApiPlugin;
