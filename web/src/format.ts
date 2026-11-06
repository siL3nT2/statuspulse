export function formatUptime(value: number | null): string {
  return value === null ? 'no data' : `${value}%`
}

export function formatMs(value: number | null): string {
  return value === null ? '–' : `${value} ms`
}

export function timeAgo(iso: string, now: Date = new Date()): string {
  const seconds = Math.max(
    0,
    Math.round((now.getTime() - new Date(iso).getTime()) / 1000),
  )

  if (seconds < 60) return 'just now'

  const minutes = Math.floor(seconds / 60)
  if (minutes < 60) return `${minutes} min ago`

  const hours = Math.floor(minutes / 60)
  if (hours < 24) return `${hours} h ago`

  return `${Math.floor(hours / 24)} d ago`
}

export function isStale(
  generatedAt: string,
  maxAgeMinutes: number,
  now: Date = new Date(),
): boolean {
  const ageMinutes = (now.getTime() - new Date(generatedAt).getTime()) / 60000
  return ageMinutes > maxAgeMinutes
}

export function formatDuration(minutes: number | null): string {
  if (minutes === null) return 'ongoing'

  const rounded = Math.round(minutes)
  if (rounded < 1) return '< 1 min'
  if (rounded < 60) return `${rounded} min`

  const hours = Math.floor(rounded / 60)
  const rest = rounded % 60
  return rest === 0 ? `${hours} h` : `${hours} h ${rest} min`
}