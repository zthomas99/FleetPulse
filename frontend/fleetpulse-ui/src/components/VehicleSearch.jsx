function VehicleSearch({ vehicleId, onVehicleIdChange, onSearch }) {
    return (
        <div className="search-panel">
            <label htmlFor="vehicleId" className="sr-only">
                Vehicle ID
            </label>
            <input
                id="vehicleId"
                type="text"
                placeholder="Enter vehicle ID"
                value={vehicleId}
                onChange={(e) => onVehicleIdChange(e.target.value.toUpperCase())}
            />
            <button type="button" onClick={onSearch}>
                Search
            </button>
        </div>
    );
}

export default VehicleSearch;
