import { describe, it, expect, vi } from "vitest";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import VehicleSearch from "../components/VehicleSearch";

describe("VehicleSearch", () => {
  it("renders input and search button", () => {
    const mockOnSearch = vi.fn();
    const mockOnVehicleIdChange = vi.fn();

    render(
      <VehicleSearch
        vehicleId=""
        onVehicleIdChange={mockOnVehicleIdChange}
        onSearch={mockOnSearch}
      />
    );

    expect(screen.getByPlaceholderText("Enter vehicle ID")).toBeInTheDocument();
    expect(screen.getByRole("button", { name: /search/i })).toBeInTheDocument();
  });

  it("calls onVehicleIdChange when input changes and converts to uppercase", async () => {
    const user = userEvent.setup();
    const mockOnSearch = vi.fn();
    const mockOnVehicleIdChange = vi.fn();

    render(
      <VehicleSearch
        vehicleId=""
        onVehicleIdChange={mockOnVehicleIdChange}
        onSearch={mockOnSearch}
      />
    );

    const input = screen.getByPlaceholderText("Enter vehicle ID");
    await user.type(input, "abc");

    // Verify callback was called multiple times as user typed
    expect(mockOnVehicleIdChange).toHaveBeenCalled();
    // Verify at least one call converted to uppercase
    const callsWithUppercase = mockOnVehicleIdChange.mock.calls.filter((call) =>
      call[0].match(/[A-Z]/)
    );
    expect(callsWithUppercase.length).toBeGreaterThan(0);
  });

  it("calls onSearch when search button is clicked", async () => {
    const user = userEvent.setup();
    const mockOnSearch = vi.fn();
    const mockOnVehicleIdChange = vi.fn();

    render(
      <VehicleSearch
        vehicleId="V123"
        onVehicleIdChange={mockOnVehicleIdChange}
        onSearch={mockOnSearch}
      />
    );

    const button = screen.getByRole("button", { name: /search/i });
    await user.click(button);

    expect(mockOnSearch).toHaveBeenCalledTimes(1);
  });

  it("displays the current vehicleId value", () => {
    const mockOnSearch = vi.fn();
    const mockOnVehicleIdChange = vi.fn();

    render(
      <VehicleSearch
        vehicleId="V123"
        onVehicleIdChange={mockOnVehicleIdChange}
        onSearch={mockOnSearch}
      />
    );

    const input = screen.getByPlaceholderText("Enter vehicle ID");
    expect(input).toHaveValue("V123");
  });

  it("updates input value when prop changes", () => {
    const mockOnSearch = vi.fn();
    const mockOnVehicleIdChange = vi.fn();

    const { rerender } = render(
      <VehicleSearch
        vehicleId="V123"
        onVehicleIdChange={mockOnVehicleIdChange}
        onSearch={mockOnSearch}
      />
    );

    let input = screen.getByPlaceholderText("Enter vehicle ID");
    expect(input).toHaveValue("V123");

    rerender(
      <VehicleSearch
        vehicleId="V456"
        onVehicleIdChange={mockOnVehicleIdChange}
        onSearch={mockOnSearch}
      />
    );

    input = screen.getByPlaceholderText("Enter vehicle ID");
    expect(input).toHaveValue("V456");
  });
});
