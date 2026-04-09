(function () {
    // Ждём полной загрузки Swagger UI
    function waitForSwaggerUI() {
        return new Promise((resolve) => {
            // Проверяем каждые 500 мс
            const interval = setInterval(() => {
                // Новый способ получения Swagger UI объекта
                if (window.swaggerUi && window.swaggerUi.api) {
                    clearInterval(interval);
                    resolve(window.swaggerUi);
                }
                // Альтернативный способ
                if (window.ui && window.ui.authActions) {
                    clearInterval(interval);
                    resolve(window.ui);
                }
            }, 500);
        });
    }

    // Перехватываем fetch запросы
    const originalFetch = window.fetch;
    window.fetch = function () {
        const fetchPromise = originalFetch.apply(this, arguments);
        const [url, config] = arguments;

        if (url && url.includes('/api/auth/login') && config && config.method === 'POST') {
            fetchPromise.then(response => {
                if (response.ok) {
                    response.clone().json().then(async data => {
                        if (data && data.accessToken) {
                            const ui = await waitForSwaggerUI();

                            if (ui && ui.authActions) {
                                // Правильный формат для авторизации
                                ui.authActions.authorize({
                                    Bearer: {  // ← Имя должно совпадать с SecurityDefinition
                                        name: "Bearer",
                                        value: data.accessToken,
                                        schema: {
                                            type: "http",
                                            scheme: "Bearer",
                                            bearerFormat: "JWT"
                                        }
                                    }
                                });
                                console.log("✅ Swagger UI authorized automatically");
                            } else {
                                console.warn("⚠️ Swagger UI not found");
                            }
                        }
                    });
                }
            });
        }
        return fetchPromise;
    };
})();