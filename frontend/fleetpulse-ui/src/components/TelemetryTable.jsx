function TelemetryTable({ history, formatTimestamp }) {
    return (
        <section className="card history-card">
            <div className="card-header">
                <h2>History</h2>
                <p className="section-subtitle">Recent telemetry events for this vehicle</p>
            </div>
            <div className="table-wrapper">
                <table>
                    <thead>
                        <tr>
                            <th>Timestamp</th>
                            <th>Speed</th>
                            <th>Engine Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        {history.map((event) => (
                            <tr key={event.id}>
                                <td>{formatTimestamp(event.timestamp)}</td>
                                <td>{event.speedMph} mph</td>
                                <td>{event.engineStatus}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </section>
    );
}

export default TelemetryTable;
