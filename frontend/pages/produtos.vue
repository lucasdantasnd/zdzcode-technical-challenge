<template>
  <div>
    <!-- Top Actions -->
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem;">
      <h2 style="font-size: 1.25rem; font-weight: 600; color: var(--text-primary);">
        Todos os Produtos ({{ produtos.length }})
      </h2>
      <button class="btn btn-primary" @click="openCreateModal">
        <span style="font-size: 1.1rem; line-height: 1;">+</span>
        Novo Produto
      </button>
    </div>

    <!-- Loading State -->
    <div v-if="loading || loadingCats" style="display: flex; justify-content: center; padding: 4rem;">
      <div style="color: var(--text-muted); font-weight: 500;">Carregando dados...</div>
    </div>

    <!-- Empty State -->
    <div v-else-if="produtos.length === 0" class="card empty-state">
      <div class="empty-icon">📦</div>
      <h3>Nenhum produto cadastrado</h3>
      <p>Gerencie o estoque cadastrando seus primeiros produtos no catálogo.</p>
      <button class="btn btn-primary" style="margin-top: 1.5rem;" @click="openCreateModal">
        Cadastrar Primeiro Produto
      </button>
    </div>

    <!-- Data Table Grid -->
    <div v-else class="grid-container">
      <table class="data-table">
        <thead>
          <tr>
            <th style="width: 80px;">ID</th>
            <th>Nome</th>
            <th>Descrição</th>
            <th>Preço</th>
            <th>Categoria</th>
            <th style="width: 150px; text-align: right;">Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="prod in produtos" :key="prod.id">
            <td style="font-family: var(--font-title); font-weight: 600; color: var(--text-muted);">
              #{{ prod.id }}
            </td>
            <td style="font-weight: 600; color: var(--text-primary);">
              {{ prod.nome }}
            </td>
            <td style="color: var(--text-muted); max-width: 250px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;">
              {{ prod.descricao || '—' }}
            </td>
            <td>
              <span class="badge badge-price">
                R$ {{ formatPreco(prod.preco) }}
              </span>
            </td>
            <td>
              <span class="badge">
                {{ prod.categoriaNome || 'Sem Categoria' }}
              </span>
            </td>
            <td style="text-align: right;">
              <div class="actions-cell" style="justify-content: flex-end;">
                <button
                  class="btn btn-secondary btn-icon"
                  title="Editar Produto"
                  @click="openEditModal(prod)"
                >
                  ✏️
                </button>
                <button
                  class="btn btn-danger btn-icon"
                  title="Excluir Produto"
                  @click="triggerDelete(prod)"
                >
                  🗑️
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Produto Modal -->
    <ProdutoModal
      :show="showFormModal"
      :produto="selectedProduto"
      :categorias="categorias"
      @close="showFormModal = false"
      @save="handleSave"
    />

    <!-- Confirm Delete Dialog -->
    <ConfirmDialog
      :show="showDeleteDialog"
      title="Excluir Produto"
      :message="`Tem certeza que deseja excluir o produto '${produtoToDelete?.nome}'? Esta ação não poderá ser desfeita.`"
      @confirm="handleConfirmDelete"
      @cancel="showDeleteDialog = false"
    />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useProdutos } from '~/composables/useProdutos'
import { useCategorias } from '~/composables/useCategorias'
import ProdutoModal from '~/components/ProdutoModal.vue'
import ConfirmDialog from '~/components/ConfirmDialog.vue'

const {
  produtos,
  loading,
  fetchProdutos,
  createProduto,
  updateProduto,
  deleteProduto
} = useProdutos()

const {
  categorias,
  loading: loadingCats,
  fetchCategorias
} = useCategorias()

const showFormModal = ref(false)
const showDeleteDialog = ref(false)
const selectedProduto = ref(null)
const produtoToDelete = ref(null)

onMounted(async () => {
  await Promise.all([
    fetchProdutos(),
    fetchCategorias() // Fetch categories to populate selects dynamically (AC 08)
  ])
})

const formatPreco = (val) => {
  if (val === undefined || val === null) return '0,00'
  return parseFloat(val).toLocaleString('pt-BR', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  })
}

const openCreateModal = () => {
  selectedProduto.value = null
  showFormModal.value = true
}

const openEditModal = (prod) => {
  selectedProduto.value = prod
  showFormModal.value = true
}

const handleSave = async (data) => {
  if (data.id) {
    // Update (PUT)
    return await updateProduto(data.id, data.nome, data.preco, data.categoriaId, data.descricao)
  } else {
    // Create (POST)
    return await createProduto(data.nome, data.preco, data.categoriaId, data.descricao)
  }
}

const triggerDelete = (prod) => {
  produtoToDelete.value = prod
  showDeleteDialog.value = true
}

const handleConfirmDelete = async () => {
  if (!produtoToDelete.value) return
  const success = await deleteProduto(produtoToDelete.value.id)
  if (success) {
    showDeleteDialog.value = false
    produtoToDelete.value = null
  }
}
</script>
