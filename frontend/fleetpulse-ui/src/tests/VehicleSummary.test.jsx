import { describe, it, expect } from "vitest";
import { render, screen } from "@testing-library/react";
import VehicleSummary from "../components/VehicleSummary";

describe("VehicleSummary", () => {
  const mockFormatTimestamp = (value) => {
    const date = new Date(value);
    return `${date.getMonth() + 1}-${date.getDate()}-${date.getFullYear()} ${date.getHours()}:${date.getMinutes()}`;
  };

  const mockLatestEvent = {
    vehicleId: "V123",
    speedMph: 55,
    engineStatus: "On",
    timestamp: "2026-06-08T10:30:00Z",
  };

  const mockSummary = {
    eventCount: 10,
    maxSpeed: 80,
    averageSpeed: 60,
    lastSeen: "2026-06-08T10:30:00Z",
  };

  it("renders latest event card", () => {
    render(
      <VehicleSummary
        latestEvent={mockLatestEvent}
        summary={mockSummary}
        formatTimestamp={mockFormatTimestamp}
      />
    );

    expect(screen.getByText("Latest Event")).toBeInTheDocument();
    expect(screen.getByText("V123")).toBeInTheDocument();
    expect(screen.getByText("55 mph")).toBeInTheDocument();
    expect(screen.getByText("On")).toBeInTheDocument();
  });

  it("renders summary metrics when summary is provided", () => {
    render(
      <VehicleSummary
        latestEvent={mockLatestEvent}
        summary={mockSummary}
        formatTimestamp={mockFormatTimestamp}
      />
    );

    expect(screen.getByText("Summary")).toBeInTheDocument();
    expect(screen.getByText("10")).toBeInTheDocument();
    expect(screen.getByText("80 mph")).toBeInTheDocument();
    expect(screen.getByText("60 mph")).toBeInTheDocument();
  });

  it("displays formatted timestamps", () => {
    render(
      <VehicleSummary
        latestEvent={mockLatestEvent}
        summary={mockSummary}
        formatTimestamp={mockFormatTimestamp}
      />
    );

    const timestamp = mockFormatTimestamp(mockLatestEvent.timestamp);
    // Since the timestamp appears in both Latest Event and Summary, get all and check at least one
    const timestampElements = screen.getAllByText(timestamp);
    expect(timestampElements.length).toBeGreaterThanOrEqual(1);
  });

  it("renders detail labels correctly", () => {
    render(
      <VehicleSummary
        latestEvent={mockLatestEvent}
        summary={mockSummary}
        formatTimestamp={mockFormatTimestamp}
      />
    );

    expect(screen.getByText("Vehicle")).toBeInTheDocument();
    expect(screen.getByText("Current Speed")).toBeInTheDocument();
    expect(screen.getByText("Status")).toBeInTheDocument();
    expect(screen.getByText("Timestamp")).toBeInTheDocument();
  });

  it("renders summary metric labels", () => {
    render(
      <VehicleSummary
        latestEvent={mockLatestEvent}
        summary={mockSummary}
        formatTimestamp={mockFormatTimestamp}
      />
    );

    expect(screen.getByText("Event Count")).toBeInTheDocument();
    expect(screen.getByText("Max Speed")).toBeInTheDocument();
    expect(screen.getByText("Average Speed")).toBeInTheDocument();
    expect(screen.getByText("Last Seen")).toBeInTheDocument();
  });

  it("handles null summary gracefully", () => {
    render(
      <VehicleSummary
        latestEvent={mockLatestEvent}
        summary={null}
        formatTimestamp={mockFormatTimestamp}
      />
    );

    expect(screen.getByText("Latest Event")).toBeInTheDocument();
    expect(screen.queryByText("Summary")).not.toBeInTheDocument();
  });
});
