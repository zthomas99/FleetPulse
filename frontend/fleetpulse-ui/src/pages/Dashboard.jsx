import { useEffect, useState } from "react";

import VehicleSearch from "../components/VehicleSearch";
import VehicleSummary from "../components/VehicleSummary";
import TelemetryTable from "../components/TelemetryTable";
import VehicleList from "../components/VehicleList";

import {
    getLatestVehicle,
    getVehicleHistory,
    getVehicleSummary,
    getVehicleIds
} from "../api/telemetryApi";

function Dashboard() {
    const [vehicleId, setVehicleId] = useState("");
    const [latestEvent, setLatestEvent] = useState(null);
    const [history, setHistory] = useState([]);
    const [summary, setSummary] = useState(null);
    const [vehicleIds, setVehicleIds] = useState([]);
    const [error, setError] = useState("");

    const handleSearch = async () => {
        setError("");
        setLatestEvent(null);
        setHistory([]);
        setSummary(null);

        const normalizedVehicleId = vehicleId.trim().toUpperCase();

        try {
            const latestResponse = await getLatestVehicle(normalizedVehicleId);
            const historyResponse = await getVehicleHistory(normalizedVehicleId);
            const summaryResponse = await getVehicleSummary(normalizedVehicleId);

            setLatestEvent(latestResponse.data);
            setHistory(historyResponse.data);
            setSummary(summaryResponse.data);
        } catch {
            setError("Vehicle not found or API request failed.");
        }
    };

    useEffect(() => {
        let cancelled = false;

        (async () => {
            try {
                const response = await getVehicleIds();
                if (!cancelled) {
                    // only set state if component is still mounted
                    setVehicleIds(response.data);
                }
            } catch {
                // ignore errors for now
            }
        })();

        return () => {
            cancelled = true;
        };
    }, []);

    const formatTimestamp = (value) => {
        if (!value) {
            return "";
        }

        const date = new Date(value);
        if (Number.isNaN(date.getTime())) {
            return value;
        }

        const pad = (number) => String(number).padStart(2, "0");
        const month = pad(date.getMonth() + 1);
        const day = pad(date.getDate());
        const year = date.getFullYear();
        const hours = pad(date.getHours());
        const minutes = pad(date.getMinutes());

        return `${month}-${day}-${year} ${hours}:${minutes}`;
    };

    return (
        <main className="dashboard-page">
            <section className="dashboard-header">
                <div className="dashboard-intro">
                    <span className="eyebrow">FleetPulse</span>
                    <h1>Vehicle Intelligence Dashboard</h1>
                    <p className="dashboard-copy">
                        Search a vehicle ID to see live telemetry, summary insights, and recent history
                        in a polished fleet dashboard.
                    </p>
                </div>
                <VehicleSearch
                    vehicleId={vehicleId}
                    onVehicleIdChange={setVehicleId}
                    onSearch={handleSearch}
                />
            </section>

            <VehicleList vehicleIds={vehicleIds} />

            {error && <div className="alert-card">{error}</div>}

            {latestEvent && (
                <VehicleSummary
                    latestEvent={latestEvent}
                    summary={summary}
                    formatTimestamp={formatTimestamp}
                />
            )}

            {history.length > 0 && (
                <TelemetryTable history={history} formatTimestamp={formatTimestamp} />
            )}
        </main>
    );
}

export default Dashboard;
