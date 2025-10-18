
document.addEventListener("DOMContentLoaded", async function () {
    const width = 1000, height = 700;

    const svg = d3.select("#haltestellen-map")
        .append("svg")
        .attr("width", width)
        .attr("height", height)
        .call(d3.zoom().on("zoom", function (event) {
            svg.attr("transform", event.transform);
        }))
        .append("g");

    const tooltip = d3.select("body")
        .append("div")
        .attr("class", "tooltip")
        .style("position", "absolute")
        .style("visibility", "hidden")
        .style("background", "#fff")
        .style("border", "1px solid #ccc")
        .style("padding", "8px")
        .style("border-radius", "8px")
        .style("box-shadow", "0 2px 5px rgba(0,0,0,0.3)")
        .style("font-size", "13px")
        .style("pointer-events", "none");

    const stopsResponse = await fetch("/api/StopApi");
    const stopData = await stopsResponse.json();

    const nodes = stopData.stops.map(stop => ({
        id: stop.id,
        name: stop.name,
        x: Math.random() * width,
        y: Math.random() * height,
        group: stop.lines || "default"
    }));

    const connectionsResponse = await fetch("/api/StopConnection");
    const connectionData = await connectionsResponse.json();

    const links = connectionData.map(conn => ({
        source: conn.fromStopId,
        target: conn.toStopId
    }));

    const simulation = d3.forceSimulation(nodes)
        .force("link", d3.forceLink(links).id(d => d.id).distance(100))
        .force("charge", d3.forceManyBody().strength(-200))
        .force("center", d3.forceCenter(width / 2, height / 2));

    const link = svg.append("g")
        .attr("stroke", "#aaa")
        .attr("stroke-width", 1.5)
        .selectAll("line")
        .data(links)
        .join("line");

    const node = svg.append("g")
        .attr("stroke", "#fff")
        .attr("stroke-width", 1.5)
        .selectAll("circle")
        .data(nodes)
        .join("circle")
        .attr("r", 10)
        .attr("fill", d => d3.schemeCategory10[d.group.length % 10])
        .on("mouseover", function (event, d) {
            tooltip.html(`<b>${d.name}</b><br/>Linien: ${d.group}`)
                .style("top", (event.pageY + 10) + "px")
                .style("left", (event.pageX + 10) + "px")
                .style("visibility", "visible");
        })
        .on("mouseout", () => tooltip.style("visibility", "hidden"));

    simulation.on("tick", () => {
        link
            .attr("x1", d => d.source.x)
            .attr("y1", d => d.source.y)
            .attr("x2", d => d.target.x)
            .attr("y2", d => d.target.y);

        node
            .attr("cx", d => d.x)
            .attr("cy", d => d.y);
    });
});
