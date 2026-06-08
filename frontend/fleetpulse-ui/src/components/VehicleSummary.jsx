function VehicleSummary({ latestEvent, summary, formatTimestamp }) {
    return (
        <>
            <section className="card latest-card">
                <div className="card-header">
                    <h2>Latest Event</h2>
                    <p className="section-subtitle">Real-time vehicle status and telemetry</p>
                </div>
                <div className="detail-list">
                    <div className="detail-item">
                        <strong>Vehicle</strong>
                        <span>{latestEvent.vehicleId}</span>
                    </div>
                    <div className="detail-item">
                        <strong>Current Speed</strong>
                        <span>{latestEvent.speedMph} mph</span>
                    </div>
                    <div className="detail-item">
                        <strong>Status</strong>
                        <span className="status-pill">{latestEvent.engineStatus}</span>
                    </div>
                    <div className="detail-item">
                        <strong>Timestamp</strong>
                        <span>{formatTimestamp(latestEvent.timestamp)}</span>
                    </div>
                </div>
            </section>

            {summary && (
                <section className="card summary-card">
                    <div className="card-header">
                        <h2>Summary</h2>
                        <p className="section-subtitle">Aggregate metrics from recent vehicle activity</p>
                    </div>
                    <div className="summary-grid">
                        <div className="metric-card">
                            <div className="metric-label">Event Count</div>
                            <div className="metric-value">{summary.eventCount}</div>
                        </div>
                        <div className="metric-card">
                            <div className="metric-label">Max Speed</div>
                            <div className="metric-value">{summary.maxSpeed} mph</div>
                        </div>
                        <div className="metric-card">
                            <div className="metric-label">Average Speed</div>
                            <div className="metric-value">{summary.averageSpeed} mph</div>
                        </div>
                        <div className="metric-card">
                            <div className="metric-label">Last Seen</div>
                            <div className="metric-value">{formatTimestamp(summary.lastSeen)}</div>
                        </div>
                    </div>
                </section>
            )}
        </>
    );
}

export default VehicleSummary;
