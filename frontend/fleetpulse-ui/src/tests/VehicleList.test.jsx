import { describe, it, expect } from "vitest";
import { render, screen } from "@testing-library/react";
import VehicleList from "../components/VehicleList";

describe("VehicleList", () => {
  it("renders vehicle ID pills for each ID in array", () => {
    const vehicleIds = ["V123", "V456", "V789"];

    render(<VehicleList vehicleIds={vehicleIds} />);

    vehicleIds.forEach((id) => {
      expect(screen.getByText(id)).toBeInTheDocument();
    });
  });

  it("renders 'No vehicles found yet' when array is empty", () => {
    render(<VehicleList vehicleIds={[]} />);

    expect(screen.getByText("No vehicles found yet.")).toBeInTheDocument();
  });

  it("does not render pills when array is empty", () => {
    const { container } = render(<VehicleList vehicleIds={[]} />);

    const pills = container.querySelectorAll(".vehicle-id-pill");
    expect(pills.length).toBe(0);
  });

  it("renders correct number of pills", () => {
    const vehicleIds = ["V001", "V002", "V003", "V004", "V005"];
    const { container } = render(<VehicleList vehicleIds={vehicleIds} />);

    const pills = container.querySelectorAll(".vehicle-id-pill");
    expect(pills.length).toBe(vehicleIds.length);
  });

  it("renders vehicle list container", () => {
    const { container } = render(<VehicleList vehicleIds={["V123"]} />);

    const listContainer = container.querySelector(".vehicle-id-grid");
    expect(listContainer).toBeInTheDocument();
  });

  it("maintains order of vehicle IDs", () => {
    const vehicleIds = ["V100", "V200", "V300"];
    const { container } = render(<VehicleList vehicleIds={vehicleIds} />);

    const pills = container.querySelectorAll(".vehicle-id-pill");
    pills.forEach((pill, index) => {
      expect(pill).toHaveTextContent(vehicleIds[index]);
    });
  });

  it("handles single vehicle ID", () => {
    render(<VehicleList vehicleIds={["V123"]} />);

    expect(screen.getByText("V123")).toBeInTheDocument();
    expect(screen.queryByText("No vehicles found yet")).not.toBeInTheDocument();
  });

  it("handles large number of vehicle IDs", () => {
    const vehicleIds = Array.from({ length: 50 }, (_, i) =>
      `V${String(i + 1).padStart(3, "0")}`
    );
    const { container } = render(<VehicleList vehicleIds={vehicleIds} />);

    const pills = container.querySelectorAll(".vehicle-id-pill");
    expect(pills.length).toBe(50);
  });
});
