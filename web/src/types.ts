export interface CheckResult {
  time: string
  ok: boolean
  status: number | null
  ms: number | null
  error: string | null
}

export type SiteState = 'up' | 'degraded' | 'down'

export interface UptimeSummary {
  last24Hours: number | null
  last7Days: number | null
  last30Days: number | null
}

export interface SiteStatus {
  id: string
  name: string
  url: string
  state: SiteState
  check: CheckResult
  uptime: UptimeSummary
}

export interface LatestReport {
  generatedAt: string
  sites: SiteStatus[]
}

export interface Incident {
  siteId: string
  start: string
  end: string | null
  durationMinutes: number | null
}