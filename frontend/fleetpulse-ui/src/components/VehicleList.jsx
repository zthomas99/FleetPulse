function VehicleList({ vehicleIds }) {
    return (
        <section className="card vehicle-list-card">
            <div className="card-header">
                <h2>Vehicles in Database</h2>
                <p className="section-subtitle">Available vehicle IDs from the backend</p>
            </div>
            {vehicleIds.length === 0 ? (
                <p>No vehicles found yet.</p>
            ) : (
                <ul className="vehicle-id-grid">
                    {vehicleIds.map((vehicleId) => (
                        <li key={vehicleId} className="vehicle-id-pill">
                            {vehicleId}
                        </li>
                    ))}
                </ul>
            )}
        </section>
    );
}

export default VehicleList;
