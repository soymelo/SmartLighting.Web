const activeEffects = new WeakMap();

export function startLightTrail(container, canvas) {
    stopLightTrail(canvas);

    const reducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)");
    if (reducedMotion.matches) {
        return;
    }

    const context = canvas.getContext("2d");
    if (!context) {
        return;
    }

    const controller = new AbortController();
    const trail = [];
    let pointer = null;
    let frame = 0;
    let width = 0;
    let height = 0;
    let lastSample = 0;

    function resize() {
        const rect = container.getBoundingClientRect();
        const scale = Math.min(window.devicePixelRatio || 1, 2);
        width = rect.width;
        height = rect.height;
        canvas.width = Math.round(width * scale);
        canvas.height = Math.round(height * scale);
        context.setTransform(scale, 0, 0, scale, 0, 0);
        schedule();
    }

    function glow(x, y, radius, opacity) {
        const gradient = context.createRadialGradient(x, y, 0, x, y, radius);
        gradient.addColorStop(0, `rgba(146, 178, 250, ${opacity})`);
        gradient.addColorStop(0.35, `rgba(103, 220, 176, ${opacity * 0.45})`);
        gradient.addColorStop(1, "rgba(103, 220, 176, 0)");
        context.fillStyle = gradient;
        context.fillRect(x - radius, y - radius, radius * 2, radius * 2);
    }

    function draw(now) {
        frame = 0;
        context.clearRect(0, 0, width, height);

        while (trail.length && now - trail[0].time > 650) {
            trail.shift();
        }

        for (const point of trail) {
            const strength = 1 - (now - point.time) / 650;
            glow(point.x, point.y, 52, 0.13 * strength);
        }

        if (pointer) {
            glow(pointer.x, pointer.y, 120, 0.2);
        }

        if (pointer || trail.length) {
            schedule();
        }
    }

    function schedule() {
        if (!frame && document.visibilityState === "visible") {
            frame = requestAnimationFrame(draw);
        }
    }

    function move(event) {
        if (event.pointerType === "touch") {
            return;
        }

        const rect = container.getBoundingClientRect();
        pointer = { x: event.clientX - rect.left, y: event.clientY - rect.top };
        const now = performance.now();

        if (now - lastSample >= 16) {
            trail.push({ ...pointer, time: now });
            if (trail.length > 32) {
                trail.shift();
            }
            lastSample = now;
        }

        schedule();
    }

    function leave() {
        pointer = null;
        schedule();
    }

    function visibilityChanged() {
        if (document.visibilityState === "visible") {
            schedule();
        } else {
            cancelAnimationFrame(frame);
            frame = 0;
        }
    }

    function motionChanged() {
        if (reducedMotion.matches) {
            stopLightTrail(canvas);
        }
    }

    const observer = new ResizeObserver(resize);
    observer.observe(container);
    container.addEventListener("pointermove", move, { signal: controller.signal });
    container.addEventListener("pointerleave", leave, { signal: controller.signal });
    document.addEventListener("visibilitychange", visibilityChanged, { signal: controller.signal });
    reducedMotion.addEventListener("change", motionChanged, { signal: controller.signal });

    activeEffects.set(canvas, () => {
        controller.abort();
        observer.disconnect();
        cancelAnimationFrame(frame);
        context.clearRect(0, 0, width, height);
    });
    resize();
}

export function stopLightTrail(canvas) {
    activeEffects.get(canvas)?.();
    activeEffects.delete(canvas);
}
