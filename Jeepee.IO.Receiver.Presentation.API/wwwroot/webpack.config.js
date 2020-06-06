const path = require("path");


module.exports = env => {
    return {
        entry: "./src/index.tsx",
        mode: env.production === "production" ? "production" : "development",
        output: {
            path: path.resolve(__dirname, "dist"),
            filename: "bundle.js",
            publicPath: "/output"
        },
        resolve: {
            extensions: [".ts", ".tsx", ".js"],
            alias: {
                "react": "preact/compat",
                "react-dom": "preact/compat",
                "@config/production": path.join(__dirname, "src", "config", env.production === "production" ? "production" : "development"),
            }
        },
        module: {
            rules: [
                { 
                    test: /\.tsx?$/, 
                    loader: 'ts-loader', 
                    exclude: /node_modules/ 
                }
            ]
        }
    }
}