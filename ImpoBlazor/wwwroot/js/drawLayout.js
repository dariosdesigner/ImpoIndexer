window.drawLayout = (params) => {
    console.log('Faces recebidas:', params.faces);
    const canvas = document.getElementById("layoutCanvas");
    if (!canvas) return;
    const ctx = canvas.getContext("2d");
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    const margin = 20; // margem fixa (pixels)
    const scale = params.canvasScale; // fator de escala
    const gap = params.gapBetween * scale;
    const sheetMargin = params.sheetMargin * scale;
    const pointsPerMillimeter = 2.83465; // 1 mm = 2.83465 pts

    // Para cada face definida
    params.faces.forEach(faceObj => {
        const faceLabel = (faceObj.face || "front").toUpperCase();
        const sheet = faceObj.sheet;
        let offsetY = margin;
        // Determina se é duplex (face "back" existe)
        const isDuplex = params.faces.some(f => (f.Face || "front").toLowerCase() === "back");
        if (isDuplex) {
            // Cada folha tem 2 faces empilhadas
            offsetY += sheet * 2 * (params.outputHeight * scale + sheetMargin);
            if ((faceObj.face || "front").toLowerCase() === "back") {
                offsetY += params.outputHeight * scale + sheetMargin;
            }
        } else {
            offsetY += sheet * (params.outputHeight * scale + sheetMargin);
        }
        const offsetX = margin;

        // Desenha o grid da face
        drawFace(ctx, offsetX, offsetY, params.outputWidth * scale, params.outputHeight * scale, gap, params.cols, params.rows, faceObj.order);
        // Desenha as cotas (convertendo de pts para mm)
        drawCotas(ctx, offsetX, offsetY, params.outputWidth * scale, params.outputHeight * scale, pointsPerMillimeter);
        // Rótulo da face
        ctx.font = "14px sans-serif";
        ctx.fillStyle = "black";
        ctx.fillText(`${faceLabel} - Folha ${sheet + 1}`, offsetX, offsetY - 10);
    });
};

function drawFace(ctx, startX, startY, faceWidth, faceHeight, gap, cols, rows, order) {
    if (!order || !Array.isArray(order) || order.length === 0) {
        console.error("Order array is not defined or empty:", order);
        return;
    }
    const cellW = (faceWidth - (cols - 1) * gap) / cols;
    const cellH = (faceHeight - (rows - 1) * gap) / rows;

    for (let r = 0; r < rows; r++) {
        for (let c = 0; c < cols; c++) {
            const idx = r * cols + c;
            if (idx >= order.length) continue;
            const x = startX + c * (cellW + gap);
            const y = startY + r * (cellH + gap);
            // Fundo (cinza)
            ctx.fillStyle = "lightgray";
            ctx.fillRect(x, y, cellW, cellH);
            // Borda (preto)
            ctx.strokeStyle = "black";
            ctx.lineWidth = 1;
            ctx.strokeRect(x, y, cellW, cellH);
            // Número centralizado
            const text = order[idx].toString();
            ctx.font = "12px sans-serif";
            ctx.fillStyle = "black";
            const metrics = ctx.measureText(text);
            const textX = x + (cellW - metrics.width) / 2;
            const textY = y + cellH / 2 + 6;
            ctx.fillText(text, textX, textY);
        }
    }
}

function drawCotas(ctx, startX, startY, faceWidth, faceHeight, pointsPerMillimeter) {
    const margin = 10;
    const widthMM = faceWidth / pointsPerMillimeter;
    const heightMM = faceHeight / pointsPerMillimeter;
    // Linha horizontal acima da face
    const lineY = startY - margin;
    ctx.strokeStyle = "black";
    ctx.lineWidth = 1;
    ctx.beginPath();
    ctx.moveTo(startX, lineY);
    ctx.lineTo(startX + faceWidth, lineY);
    ctx.stroke();
    ctx.font = "12px sans-serif";
    ctx.fillStyle = "black";
    const widthText = widthMM.toFixed(0) + " mm";
    const textWidth = ctx.measureText(widthText).width;
    ctx.fillText(widthText, startX + (faceWidth - textWidth) / 2, lineY - 5);

    // Linha vertical à esquerda da face
    const lineX = startX - margin;
    ctx.beginPath();
    ctx.moveTo(lineX, startY);
    ctx.lineTo(lineX, startY + faceHeight);
    ctx.stroke();
    const heightText = heightMM.toFixed(0) + " mm";
    ctx.save();
    ctx.translate(lineX - 5, startY + faceHeight / 2);
    ctx.rotate(-Math.PI / 2);
    const textHeight = ctx.measureText(heightText).width;
    ctx.fillText(heightText, -textHeight / 2, 0);
    ctx.restore();
}
