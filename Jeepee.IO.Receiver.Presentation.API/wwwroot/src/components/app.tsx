import React, { FC, useState, useEffect } from "react";
import { HubConnection, HubConnectionBuilder } from "@aspnet/signalr";
import { config } from "@config/production";

interface Pin {
    number: number;
    on: boolean;
}

const App: FC = () => {
    const [connection, setConnection] = useState<HubConnection>(null);
    const [pins, setPins] = useState<Array<Pin>>([]);

    useEffect(() => {
        if (connection) {
            const start = async () => {
                connection.on("serverupdate", (number: number, on: boolean) => {
                    setPins(p => [...p, { number, on }]);
                });

                await connection.start();
            }

            start();

            return async () => await connection.stop();
        }
    }, [connection]);

    useEffect(() => {
        setConnection(new HubConnectionBuilder().withUrl(`${config.apiURL}/monitorHub`).build());
    }, []);

    return (
        <div>{pins.map((p, i) => <div key={i}>{p.number} {p.on + ""}</div>)}</div>
    );
}

export default App;