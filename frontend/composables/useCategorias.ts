import { ref } from 'vue'
import axios from 'axios'
import { useToasts } from './useToasts'

export interface Categoria {
  id: number
  nome: string
  descricao?: string
}

export function useCategorias() {
  const config = useRuntimeConfig()
  const apiBase = config.public.apiBase
  const toast = useToasts()

  const categorias = ref<Categoria[]>([])
  const loading = ref(false)

  const fetchCategorias = async () => {
    loading.value = true
    try {
      const response = await axios.get<Categoria[]>(`${apiBase}/api/categorias`)
      categorias.value = response.data
    } catch (err: any) {
      toast.error('Erro de Conexão', 'Não foi possível carregar as categorias.')
      console.error(err)
    } finally {
      loading.value = false
    }
  }

  const createCategoria = async (nome: string, descricao?: string) => {
    try {
      const response = await axios.post<Categoria>(`${apiBase}/api/categorias`, { nome, descricao })
      categorias.value.push(response.data)
      toast.success('Sucesso', `Categoria "${nome}" cadastrada com sucesso!`)
      return true
    } catch (err: any) {
      if (err.response?.status === 400) {
        const errors = err.response.data
        const errorMsg = typeof errors === 'object' ? Object.values(errors).flat().join(', ') : 'Dados inválidos.'
        toast.error('Erro de Validação', errorMsg)
      } else {
        toast.error('Erro', 'Não foi possível cadastrar a categoria.')
      }
      console.error(err)
      return false
    }
  }

  const updateCategoria = async (id: number, nome: string, descricao?: string) => {
    try {
      const response = await axios.put<Categoria>(`${apiBase}/api/categorias/${id}`, { nome, descricao })
      const index = categorias.value.findIndex(c => c.id === id)
      if (index !== -1) {
        categorias.value[index] = response.data
      }
      toast.success('Sucesso', `Categoria "${nome}" atualizada com sucesso!`)
      return true
    } catch (err: any) {
      if (err.response?.status === 400) {
        const errors = err.response.data
        const errorMsg = typeof errors === 'object' ? Object.values(errors).flat().join(', ') : 'Dados inválidos.'
        toast.error('Erro de Validação', errorMsg)
      } else {
        toast.error('Erro', 'Não foi possível atualizar a categoria.')
      }
      console.error(err)
      return false
    }
  }

  const deleteCategoria = async (id: number) => {
    try {
      await axios.delete(`${apiBase}/api/categorias/${id}`)
      categorias.value = categorias.value.filter(c => c.id !== id)
      toast.success('Sucesso', 'Categoria excluída com sucesso!')
      return true
    } catch (err: any) {
      // AC 07 — Tratamento de Erros (Capturar erro de integridade referencial e exibir mensagem amigável)
      if (err.response?.status === 409 || err.response?.status === 400) {
        const msg = err.response.data?.mensagem || 'Não é possível excluir uma categoria que possua produtos vinculados.'
        toast.error('Restrição de Integridade', msg)
      } else {
        toast.error('Erro', 'Ocorreu um erro ao excluir a categoria.')
      }
      console.error(err)
      return false
    }
  }

  return {
    categorias,
    loading,
    fetchCategorias,
    createCategoria,
    updateCategoria,
    deleteCategoria
  }
}
