import { ref } from 'vue'
import axios from 'axios'
import { useToasts } from './useToasts'

export interface Produto {
  id: number
  nome: string
  descricao?: string
  preco: number
  categoriaId: number
  categoriaNome?: string
}

export function useProdutos() {
  const config = useRuntimeConfig()
  const apiBase = config.public.apiBase
  const toast = useToasts()

  const produtos = ref<Produto[]>([])
  const loading = ref(false)

  const fetchProdutos = async () => {
    loading.value = true
    try {
      const response = await axios.get<Produto[]>(`${apiBase}/api/produtos`)
      produtos.value = response.data
    } catch (err: any) {
      toast.error('Erro de Conexão', 'Não foi possível carregar os produtos.')
      console.error(err)
    } finally {
      loading.value = false
    }
  }

  const createProduto = async (nome: string, preco: number, categoriaId: number, descricao?: string) => {
    try {
      const response = await axios.post<Produto>(`${apiBase}/api/produtos`, {
        nome,
        preco,
        categoriaId,
        descricao
      })
      produtos.value.push(response.data)
      toast.success('Sucesso', `Produto "${nome}" cadastrado com sucesso!`)
      return true
    } catch (err: any) {
      if (err.response?.status === 400) {
        const errors = err.response.data
        const errorMsg = typeof errors === 'object' ? Object.values(errors).flat().join(', ') : 'Dados inválidos.'
        toast.error('Erro de Validação', errorMsg)
      } else {
        toast.error('Erro', 'Não foi possível cadastrar o produto.')
      }
      console.error(err)
      return false
    }
  }

  const updateProduto = async (id: number, nome: string, preco: number, categoriaId: number, descricao?: string) => {
    try {
      const response = await axios.put<Produto>(`${apiBase}/api/produtos/${id}`, {
        nome,
        preco,
        categoriaId,
        descricao
      })
      const index = produtos.value.findIndex(p => p.id === id)
      if (index !== -1) {
        produtos.value[index] = response.data
      }
      toast.success('Sucesso', `Produto "${nome}" atualizado com sucesso!`)
      return true
    } catch (err: any) {
      if (err.response?.status === 400) {
        const errors = err.response.data
        const errorMsg = typeof errors === 'object' ? Object.values(errors).flat().join(', ') : 'Dados inválidos.'
        toast.error('Erro de Validação', errorMsg)
      } else {
        toast.error('Erro', 'Não foi possível atualizar o produto.')
      }
      console.error(err)
      return false
    }
  }

  const deleteProduto = async (id: number) => {
    try {
      await axios.delete(`${apiBase}/api/produtos/${id}`)
      produtos.value = produtos.value.filter(p => p.id !== id)
      toast.success('Sucesso', 'Produto excluído com sucesso!')
      return true
    } catch (err: any) {
      toast.error('Erro', 'Não foi possível excluir o produto.')
      console.error(err)
      return false
    }
  }

  return {
    produtos,
    loading,
    fetchProdutos,
    createProduto,
    updateProduto,
    deleteProduto
  }
}
