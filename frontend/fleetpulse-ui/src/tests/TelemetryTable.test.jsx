import { describe, it, expect } from "vitest";
import { render, screen } from "@testing-library/react";
import TelemetryTable from "../components/TelemetryTable";

describe("TelemetryTable", () => {
  const mockFormatTimestamp = (value) => {
    const date = new Date(value);
    return `${date.getMonth() + 1}-${date.getDate()}-${date.getFullYear()} ${date.getHours()}:${date.getMinutes()}`;
  };

  const mockHistory = [
    {
      id: 1,
      vehicleId: "V123",
      speedMph: 45,
      engineStatus: "On",
      timestamp: "2026-06-08T10:30:00Z",
    },
    {
      id: 2,
      vehicleId: "V123",
      speedMph: 55,
      engineStatus: "On",
      timestamp: "2026-06-08T10:25:00Z",
    },
    {
      id: 3,
      vehicleId: "V123",
      speedMph: 50,
      engineStatus: "Off",
      timestamp: "2026-06-08T10:20:00Z",
    },
  ];

  it("renders table with headers", () => {
    render(
      <TelemetryTable
        history={mockHistory}
        formatTimestamp={mockFormatTimestamp}
      />
    );

    expect(screen.getByText("Timestamp")).toBeInTheDocument();
    expect(screen.getByText("Speed")).toBeInTheDocument();
    expect(screen.getByText("Engine Status")).toBeInTheDocument();
  });

  it("renders all history items as table rows", () => {
    render(
      <TelemetryTable
        history={mockHistory}
        formatTimestamp={mockFormatTimestamp}
      />
    );

    expect(screen.getByText("45 mph")).toBeInTheDocument();
    expect(screen.getByText("55 mph")).toBeInTheDocument();
    expect(screen.getByText("50 mph")).toBeInTheDocument();
  });

  it("renders engine status correctly", () => {
    render(
      <TelemetryTable
        history={mockHistory}
        formatTimestamp={mockFormatTimestamp}
      />
    );

    const statusCells = screen.getAllByText(/On|Off/);
    expect(statusCells.length).toBeGreaterThanOrEqual(3);
  });

  it("applies formatTimestamp to each event", () => {
    render(
      <TelemetryTable
        history={mockHistory}
        formatTimestamp={mockFormatTimestamp}
      />
    );

    mockHistory.forEach((event) => {
      const formatted = mockFormatTimestamp(event.timestamp);
      expect(screen.getByText(formatted)).toBeInTheDocument();
    });
  });

  it("renders empty table when history is empty", () => {
    const { container } = render(
      <TelemetryTable history={[]} formatTimestamp={mockFormatTimestamp} />
    );

    const table = container.querySelector("table");
    const bodyRows = table.querySelectorAll("tbody tr");
    expect(bodyRows.length).toBe(0);
  });

  it("renders table structure correctly", () => {
    const { container } = render(
      <TelemetryTable
        history={mockHistory}
        formatTimestamp={mockFormatTimestamp}
      />
    );

    const table = container.querySelector("table");
    expect(table).toBeInTheDocument();

    const thead = table.querySelector("thead");
    expect(thead).toBeInTheDocument();

    const tbody = table.querySelector("tbody");
    expect(tbody).toBeInTheDocument();

    const bodyRows = tbody.querySelectorAll("tr");
    expect(bodyRows.length).toBe(mockHistory.length);
  });
});
