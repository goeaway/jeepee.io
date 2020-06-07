const path = require("path");


module.exports = [
    {
        entry: "./src/index.tsx",
        mode: "production",
        output: {
            path: path.resolve(__dirname, "dist"),
            filename: "bundle-production.js",
            publicPath: "/output"
        },
        resolve: {
            extensions: [".ts", ".tsx", ".js"],
            alias: {
                "react": "preact/compat",
                "react-dom": "preact/compat",
                "@config/production": path.join(__dirname, "src", "config", "production"),
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
    },
    {
        entry: "./src/index.tsx",
        mode: "development",
        output: {
            path: path.resolve(__dirname, "dist"),
            filename: "bundle-development.js",
            publicPath: "/output"
        },
        resolve: {
            extensions: [".ts", ".tsx", ".js"],
            alias: {
                "react": "preact/compat",
                "react-dom": "preact/compat",
                "@config/production": path.join(__dirname, "src", "config", "development"),
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
]
