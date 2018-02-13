import { APP_INITIALIZER } from '@angular/core';
import { AppConfig } from './app.config';

export function loadConfig(config: AppConfig) {
    return () => config.load();
}

export let AppConfigInitializerProvider = {
    provide: APP_INITIALIZER,
    useFactory: loadConfig,
    deps: [AppConfig],
    multi: true
};