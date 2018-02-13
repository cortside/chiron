export class Settings {
    deployment: string;
    app: string;
    config: string;
    build: Build;
}
export class Build {
    version: string;
    date: Date;
}