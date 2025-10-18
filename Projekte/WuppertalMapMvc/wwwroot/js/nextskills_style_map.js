document.addEventListener("DOMContentLoaded", async function () {
    const width = 800, height = 500;

    const svg = d3.select("#graph")
        .append("svg")
        .attr("width", width)
        .attr("height", height)
        .style("background", "#ffffff");

    const tooltip = d3.select("body")
        .append("div")
        .attr("class", "tooltip")
        .style("position", "absolute")
        .style("visibility", "hidden")
        .style("background", "#fff")
        .style("border", "1px solid #ccc")
        .style("padding", "10px")
        .style("border-radius", "8px")
        .style("box-shadow", "0 4px 8px rgba(0,0,0,0.1)")
        .style("font-size", "14px");

    const lineColors = {
        "603": "#e74c3c",
        "611": "#3498db",
        "628": "#27ae60",
        "601": "#8e44ad",
        "615": "#f39c12",
        "640": "#1abc9c"
    };

    const res = await fetch("/api/haltestellennetz");
    const { stops, connections } = await res.json();

    const stopById = Object.fromEntries(stops.map(s => [s.id, s]));

    // Verbindungen (mit Farbe des Start-Knotens)
    // Verbindungen (nur mit gültigen Linien)
    svg.selectAll("line")
        .data(connections.filter(conn => {
            const from = stopById[conn.fromStopId];
            const to = stopById[conn.toStopId];
            if (!from || !to) return false;
            const shared = from.lines.filter(l => to.lines.includes(l));
            return shared.length > 0;
        }))
        .enter()
        .append("line")
        .attr("x1", d => stopById[d.fromStopId]?.x ?? 0)
        .attr("y1", d => stopById[d.fromStopId]?.y ?? 0)
        .attr("x2", d => stopById[d.toStopId]?.x ?? 0)
        .attr("y2", d => stopById[d.toStopId]?.y ?? 0)
        .attr("stroke", d => {
            const from = stopById[d.fromStopId];
            const sorted = [...from.lines].sort((a, b) => parseInt(a) - parseInt(b));
            const primary = sorted.find(l => lineColors[l]) ?? null;
            return lineColors[primary] || "#aaa";
        })
        .attr("stroke-width", 4)
        .attr("stroke-linecap", "round");


    // Haltestellen (mit Spezial-Design für Hauptbahnhof)
    svg.selectAll("circle")
        .data(stops)
        .enter()
        .append("circle")
        .attr("cx", d => d.x)
        .attr("cy", d => d.y)
        .attr("r", 16)
        .attr("fill", d => {
            return d.name.toLowerCase().includes("hauptbahnhof") ? "#ccc" : (
                (() => {
                    const sorted = [...d.lines].sort((a, b) => parseInt(a) - parseInt(b));
                    const primary = sorted.find(l => lineColors[l]) ?? null;
                    return lineColors[primary] || "#999";
                })()
            );
        })
        .attr("stroke", d => {
            if (!d.name.toLowerCase().includes("hauptbahnhof")) return "#fff";
            const lines = [...d.lines].sort();
            return lineColors[lines[0]] || "#000"; // Farbe irrelevant, nur für Sichtbarkeit
        })
        .attr("stroke-width", d => d.name.toLowerCase().includes("hauptbahnhof") ? 6 : 2)
        .attr("stroke-dasharray", d => {
            if (!d.name.toLowerCase().includes("hauptbahnhof")) return null;
            const count = d.lines.length;
            return Array(count).fill(6).join(","); // z.B. "6,6,6" = segmentierter Kreisrand
        })
        .on("mouseover", (event, d) => {
            tooltip.html(`
                <strong>${d.name}</strong><br/>
                Linien: ${d.lines.join(", ")}<br/>
                ${d.description ?? ""}<br/>
                ${d.openingHours ? "Öffnungszeiten: " + d.openingHours : ""}
            `)
                .style("left", (event.pageX + 15) + "px")
                .style("top", (event.pageY - 10) + "px")
                .style("visibility", "visible");
        })
        .on("mouseout", () => {
            tooltip.style("visibility", "hidden");
        });

    // Knotennamen unter den Kreisen anzeigen
    svg.selectAll("text")
        .data(stops)
        .enter()
        .append("text")
        .attr("x", d => d.x)
        .attr("y", d => d.y - 28) // Position unter dem Kreis
        .text(d => d.name)
        .attr("text-anchor", "middle")
        .attr("font-size", "13px")
        .attr("fill", "#333")
        .style("pointer-events", "none"); // verhindert Kollision mit Maus

});
