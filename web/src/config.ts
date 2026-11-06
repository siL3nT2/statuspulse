const BASE = "https://raw.githubusercontent.com/siL3nT2/statuspulse/data";

export const LATEST_URL = `${BASE}/latest.json`;
export const INCIDENTS_URL = `${BASE}/incidents.json`;

export const CHECK_INTERVAL_MINUTES = 30;
export const STALE_AFTER_MINUTES = CHECK_INTERVAL_MINUTES * 2;
