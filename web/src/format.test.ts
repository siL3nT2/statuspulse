import { describe, expect, it } from 'vitest'
import { formatDuration, formatMs, formatUptime, isStale, timeAgo } from './format'

const NOW = new Date('2026-11-06T12:00:00Z')

describe('formatUptime', () => {
  it('shows a percentage', () => {
    expect(formatUptime(99.5)).toBe('99.5%')
  })

  it('shows "no data" when there is no value', () => {
    expect(formatUptime(null)).toBe('no data')
  })

  it('keeps a real 0% as 0%', () => {
    expect(formatUptime(0)).toBe('0%')
  })
})

describe('formatMs', () => {
  it('adds the unit', () => {
    expect(formatMs(245)).toBe('245 ms')
  })

  it('shows a dash when missing', () => {
    expect(formatMs(null)).toBe('–')
  })
})

describe('timeAgo', () => {
  it('says just now under a minute', () => {
    expect(timeAgo('2026-11-06T11:59:30Z', NOW)).toBe('just now')
  })

  it('uses minutes', () => {
    expect(timeAgo('2026-11-06T11:15:00Z', NOW)).toBe('45 min ago')
  })

  it('uses hours', () => {
    expect(timeAgo('2026-11-06T09:00:00Z', NOW)).toBe('3 h ago')
  })

  it('uses days', () => {
    expect(timeAgo('2026-11-04T12:00:00Z', NOW)).toBe('2 d ago')
  })

  it('never shows negative times', () => {
    expect(timeAgo('2026-11-06T12:05:00Z', NOW)).toBe('just now')
  })
})

describe('isStale', () => {
  it('is not stale at exactly the limit', () => {
    expect(isStale('2026-11-06T11:00:00Z', 60, NOW)).toBe(false)
  })

  it('is stale just past the limit', () => {
    expect(isStale('2026-11-06T10:59:00Z', 60, NOW)).toBe(true)
  })

  it('is not stale when fresh', () => {
    expect(isStale('2026-11-06T11:50:00Z', 60, NOW)).toBe(false)
  })
})

describe('formatDuration', () => {
  it('says ongoing when there is no end', () => {
    expect(formatDuration(null)).toBe('ongoing')
  })

  it('shows under a minute', () => {
    expect(formatDuration(0.4)).toBe('< 1 min')
  })

  it('shows minutes', () => {
    expect(formatDuration(45)).toBe('45 min')
  })

  it('shows whole hours', () => {
    expect(formatDuration(120)).toBe('2 h')
  })

  it('shows hours and minutes', () => {
    expect(formatDuration(135)).toBe('2 h 15 min')
  })

  it('rounds to the nearest minute', () => {
    expect(formatDuration(59.6)).toBe('1 h')
  })
})