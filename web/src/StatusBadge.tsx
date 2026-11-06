import type { SiteState } from './types'

const styles: Record<SiteState, { label: string; icon: string; classes: string }> = {
  up: {
    label: 'Up',
    icon: '✓',
    classes: 'bg-green-100 text-green-900 dark:bg-green-900/40 dark:text-green-200',
  },
  degraded: {
    label: 'Degraded',
    icon: '!',
    classes: 'bg-amber-100 text-amber-900 dark:bg-amber-900/40 dark:text-amber-200',
  },
  down: {
    label: 'Down',
    icon: '✕',
    classes: 'bg-red-100 text-red-900 dark:bg-red-900/40 dark:text-red-200',
  },
}

export function StatusBadge({ state }: { state: SiteState }) {
  const style = styles[state]

  return (
    <span
      className={`inline-flex items-center gap-1.5 rounded-full px-3 py-1 text-sm font-medium ${style.classes}`}
    >
      <span aria-hidden="true">{style.icon}</span>
      {style.label}
    </span>
  )
}