import { useEffect, useState } from 'react'

export type LoadState<T> =
  | { kind: 'loading' }
  | { kind: 'error'; message: string }
  | { kind: 'ready'; data: T }

export function useJson<T>(url: string): LoadState<T> {
  const [state, setState] = useState<LoadState<T>>({ kind: 'loading' })

  useEffect(() => {
    const controller = new AbortController()

    async function load() {
      try {
        const response = await fetch(url, { signal: controller.signal })

        if (!response.ok) {
          throw new Error(`Request failed with status ${response.status}`)
        }

        const data = (await response.json()) as T
        setState({ kind: 'ready', data })
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
  }, [url])

  return state
}