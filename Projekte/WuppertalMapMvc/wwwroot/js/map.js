document.addEventListener("DOMContentLoaded", function () {
    const svg = d3.select("#graph")
        .append("svg")
        .attr("width", 800)
        .attr("height", 500);

    const width = 800;
    const height = 500;

    const xScale = d3.scaleLinear().domain([7.12, 7.22]).range([0, width]);
    const yScale = d3.scaleLinear().domain([51.23, 51.28]).range([height, 0]);

    let nodes = [];
    let links = [];
    let simulation;

    async function loadData() {
        const stopResponse = await fetch("/api/stopapi");
        const stopResult = await stopResponse.json();
        nodes = stopResult.stops.map(s => ({
            id: s.id,
            name: s.name,
            lines: s.lines,
            latitude: s.latitude,
            longitude: s.longitude,
            description: s.description ?? "",
            image: s.imagePath ?? "",
            openingHours: s.openingHours ?? "",
            x: xScale(s.longitude),
            y: yScale(s.latitude),
            color: "steelblue"
        }));

        const connResponse = await fetch("/api/stopconnection");
        const connResult = await connResponse.json();
        links = connResult.map(conn => ({
            source: nodes.find(n => n.id === conn.FromId),
            target: nodes.find(n => n.id === conn.ToId)
        }));

        initializeGraph();
    }

    function initializeGraph() {
        svg.selectAll("*").remove();

        // ➔ Zwei Layer: Verbindungen + Knoten
        svg.append("g").attr("id", "links");
        svg.append("g").attr("id", "nodes");

        updateGraph();
        fillDropdowns();
    }

    function updateGraph() {
        const linkLayer = svg.select("#links");
        linkLayer
            .selectAll("line")
            .data(links)
            .join("line")
            .attr("stroke", "#aaa")
            .attr("stroke-width", 2);

        const nodeLayer = svg.select("#nodes");
        const group = nodeLayer
            .selectAll("g")
            .data(nodes)
            .join("g")
            .attr("transform", d => `translate(${d.x},${d.y})`)
            .call(d3.drag()
                .on("start", dragstarted)
                .on("drag", dragged)
                .on("end", dragended)
            );

        group.selectAll("circle")
            .data(d => [d])
            .join("circle")
            .attr("r", 8)
            .attr("fill", d => d.color)
            .on("click", (event, d) => showInfo(d));

        group.selectAll("text")
            .data(d => [d])
            .join("text")
            .text(d => d.name)
            .attr("font-size", "10px")
            .attr("dx", 10)
            .attr("dy", 4);

        simulation = d3.forceSimulation(nodes)
            .force("link", d3.forceLink(links).id(d => d.id).distance(100))
            .force("charge", d3.forceManyBody().strength(-50))
            .force("center", d3.forceCenter(width / 2, height / 2))
            .force("x", d3.forceX(width / 2).strength(0.005))
            .force("y", d3.forceY(height / 2).strength(0.005))
            .on("tick", () => {
                linkLayer.selectAll("line")
                    .attr("x1", d => d.source.x)
                    .attr("y1", d => d.source.y)
                    .attr("x2", d => d.target.x)
                    .attr("y2", d => d.target.y);

                nodeLayer.selectAll("g")
                    .attr("transform", d => `translate(${d.x},${d.y})`);
            });
    }

    function fillDropdowns() {
        const startSelect = document.getElementById("start-stop");
        const endSelect = document.getElementById("end-stop");

        if (!startSelect || !endSelect) {
            console.error("Dropdowns fehlen!");
            return;
        }

        startSelect.innerHTML = '<option value="">Wähle Start-Haltestelle...</option>';
        endSelect.innerHTML = '<option value="">Wähle Ende-Haltestelle...</option>';

        nodes.forEach(n => {
            const opt1 = document.createElement("option");
            opt1.value = n.id;
            opt1.textContent = n.name;
            startSelect.appendChild(opt1);

            const opt2 = document.createElement("option");
            opt2.value = n.id;
            opt2.textContent = n.name;
            endSelect.appendChild(opt2);
        });
    }

    function setupFormEvents() {
        const positionOption = document.getElementById("position-option");
        const startStopSelect = document.getElementById("start-stop");
        const endStopSelect = document.getElementById("end-stop");
        const endStopContainer = document.getElementById("end-stop-container");

        positionOption.addEventListener("change", function () {
            if (positionOption.value === "between") {
                endStopContainer.style.display = "block";
            } else {
                endStopContainer.style.display = "none";
            }
        });

        document.getElementById("add-stop-form").addEventListener("submit", async function (e) {
            e.preventDefault();

            const name = document.getElementById("new-name").value.trim();
            const lines = document.getElementById("new-lines").value.trim();
            const lat = parseFloat(document.getElementById("new-lat").value);
            const lng = parseFloat(document.getElementById("new-lng").value);
            const desc = document.getElementById("new-description").value.trim();
            const image = document.getElementById("new-image").value.trim();
            const openingHours = document.getElementById("new-openinghours").value.trim();

            const position = positionOption.value;
            const startId = parseInt(startStopSelect.value, 10);
            const endId = parseInt(endStopSelect.value, 10);

            if (!startId && position !== "start") {
                alert("Bitte eine Start-Haltestelle auswählen!");
                return;
            }

            const res = await fetch("/api/stopapi", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    name,
                    lines,
                    latitude: lat,
                    longitude: lng,
                    description: desc,
                    imagePath: image,
                    openingHours: openingHours,
                    isDeleted: false
                })
            });

            const savedStop = await res.json();

            const newNode = {
                id: savedStop.id,
                name: savedStop.name,
                lines: savedStop.lines,
                latitude: savedStop.latitude,
                longitude: savedStop.longitude,
                x: xScale(savedStop.longitude),
                y: yScale(savedStop.latitude),
                color: "orange",
                description: savedStop.description ?? "",
                image: savedStop.imagePath ?? "",
                openingHours: savedStop.openingHours ?? ""
            };

            nodes.push(newNode);

            if (position === "start") {
                links.push({ source: newNode, target: nodes.find(n => n.id === startId) });
            } else if (position === "end") {
                links.push({ source: nodes.find(n => n.id === startId), target: newNode });
            } else if (position === "between") {
                if (!endId) {
                    alert("Bitte eine Ende-Haltestelle auswählen!");
                    return;
                }
                links.push({ source: nodes.find(n => n.id === startId), target: newNode });
                links.push({ source: newNode, target: nodes.find(n => n.id === endId) });
            }

            updateGraph(); // 📢 Nur noch updateGraph aufrufen!
        });
    }

    function dragstarted(event, d) {
        if (!event.active) simulation.alphaTarget(0.3).restart();
        d.fx = d.x;
        d.fy = d.y;
    }

    function showInfo(d) {
        const box = document.getElementById("info-box");
        box.innerHTML = `
            <h3>${d.name}</h3>
            <p><strong>Linien:</strong> ${d.lines}</p>
            <p><strong>Öffnungszeiten:</strong> ${d.openingHours}</p>
            <p>${d.description}</p>
            ${d.image ? `<img src="/css/${d.image}" alt="${d.name}" style="width:100px;">` : ""}
        `;
        box.style.display = "block";
    }

    function dragged(event, d) {
        d.fx = Math.max(0, Math.min(width, event.x));
        d.fy = Math.max(0, Math.min(height, event.y));
    }

    function dragended(event, d) {
        if (!event.active) simulation.alphaTarget(0);
        d.fx = null;
        d.fy = null;
    }

    loadData();
    setupFormEvents();
});
