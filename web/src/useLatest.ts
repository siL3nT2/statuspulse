import { useEffect, useState } from 'react'
import { LATEST_URL } from './config'
import type { LatestReport } from './types'

type State =
  | { kind: 'loading' }
  | { kind: 'error'; message: string }
  | { kind: 'ready'; report: LatestReport }

export function useLatest(): State {
  const [state, setState] = useState<State>({ kind: 'loading' })

  useEffect(() => {
    const controller = new AbortController()

    async function load() {
      try {
        const response = await fetch(LATEST_URL, { signal: controller.signal })

        if (!response.ok) {
          throw new Error(`Request failed with status ${response.status}`)
        }

        const report = (await response.json()) as LatestReport
        setState({ kind: 'ready', report })
      } catch (error) {
        if (controller.signal.aborted) return

        setState({
          kind: 'error',
          message: error instanceof Error ? error.message : 'Unknown error',
        })
      }
    }

    load()

    return () => controller.abort()
  }, [])

  return state
}