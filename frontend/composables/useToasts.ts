import { ref } from 'vue'

export interface Toast {
  id: number
  title: string
  message: string
  type: 'success' | 'error' | 'warning' | 'info'
  duration?: number
}

const toasts = ref<Toast[]>([])
let nextId = 1

export function useToasts() {
  const addToast = (
    title: string,
    message: string,
    type: Toast['type'] = 'info',
    duration = 4000
  ) => {
    const id = nextId++
    const newToast: Toast = { id, title, message, type, duration }
    toasts.value.push(newToast)

    if (duration > 0) {
      setTimeout(() => {
        removeToast(id)
      }, duration)
    }
  }

  const removeToast = (id: number) => {
    toasts.value = toasts.value.filter(t => t.id !== id)
  }

  const success = (title: string, message: string) => addToast(title, message, 'success')
  const error = (title: string, message: string) => addToast(title, message, 'error')
  const warning = (title: string, message: string) => addToast(title, message, 'warning')
  const info = (title: string, message: string) => addToast(title, message, 'info')

  return {
    toasts,
    addToast,
    removeToast,
    success,
    error,
    warning,
    info
  }
}
