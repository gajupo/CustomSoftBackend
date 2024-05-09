<template>
    <v-row>
        <v-col cols="12" class="text-center">
            <h2>Lista de Proveedores</h2>
        </v-col>
    </v-row>
    <v-row>
        <v-col cols="6" class="text-center">
            <v-btn class="text-none text-white"
                   color="success"
                   rounded="1"
                   variant="elevated"
                   @click="registerProvider">
                Registrar Proveedor
            </v-btn>
        </v-col>
        <v-col cols="6" class="text-center">
            <v-btn class="text-none text-white"
                   color="primary"
                   rounded="1"
                   variant="elevated"
                   @click="isExportDialogOpen = true">
                Exportar Proveedores
            </v-btn>
        </v-col>
    </v-row>
    <v-row align="center" justify="center">
        <v-col cols="auto">
            <v-data-table-server v-model:items-per-page="itemsPerPage"
                                 :headers="headers"
                                 :items="proveedores.items"
                                 :items-length="proveedores.totalCount"
                                 :loading="loading"
                                 :search="search"
                                 item-value="name"
                                 @update:options="loadProveedores">
                <template v-slot:item.fechaAlta="{ item }">
                    <div class="text-end">
                        {{ formatFechaAlta(item.fechaAlta.toString()) }}
                    </div>
                </template>
                <template v-slot:item.activo="{ item }">
                    <div class="text-end">
                        <v-chip :color="item.activo ? 'green' : 'red'"
                                :text="item.activo ? 'Activo' : 'Desactivado'"
                                class="text-uppercase"
                                size="small"
                                label></v-chip>
                    </div>
                </template>
                <template v-slot:item.numFacturas="{ item }">
                    <div class="text-end">
                        {{ item.archivos.length }}
                    </div>
                </template>
                <template v-slot:item.menuActions="{ item }">
                    <ProveedorActions @item-deleted="showDialogRemoveProveedor(item.id)"
                                      @item-edited="editProveedor(item)"
                                      @add-invoices="addFacturas(item.id)"></ProveedorActions>
                </template>
            </v-data-table-server>

            <v-dialog v-model="deleteDialog"
                      max-width="400"
                      persistent>
                <v-card prepend-icon="mdi-delete"
                        text="¿Estás seguro de liminar el registro?"
                        title="Eliminar Proveedor">
                    <template v-slot:actions>
                        <v-spacer></v-spacer>

                        <v-btn @click.prevent="hiddeDialogRemoveProveedor">
                            Cancelar
                        </v-btn>

                        <v-btn @click.prevent="removeProveedor">
                            Aceptar
                        </v-btn>
                    </template>
                </v-card>
            </v-dialog>
            <ExportProveedoresDialog v-model="isExportDialogOpen"></ExportProveedoresDialog>
        </v-col>
    </v-row>
</template>


<script>
    import { parseISO, format } from 'date-fns'
    import ProveedorActions from './ProveedorActions.vue'
    import ExportProveedoresDialog from '../dialogs/ExportProveedoresDialog.vue'

    export default {
        components: {
            ProveedorActions,
            ExportProveedoresDialog
        },
        data() {
            return {
                currentIdToBeDeleted: 0,
                isExportDialogOpen: false,
                exportDialog: false,
                proveedores: [],
                page:1,
                itemsPerPage: 5,
                search: '',
                loading: true,
                headers: [
                  {
                    align: 'start',
                    key: 'nombre',
                    sortable: false,
                    title: 'Nombre Proveedor',
                  },
                  { title: 'RFC', key: 'rfc', sortable: false },
                  { title: 'Fecha Alta', key: 'fechaAlta', sortable: false },
                  { title: 'Dirección', key: 'direccion', sortable: false },
                  { title: 'Activo', key: 'activo', sortable: false },
                  { title: '#Facturas', key: 'numFacturas', sortable: false },

                  { title: 'Menú', key: 'menuActions', sortable: false },
                ]
            }
        },
        methods: {
            async loadProveedores({page, itemsPerPage}){
                 try {

                     this.loading = true
                     this.proveedores = await this.$api.get(`/proveedores?pageNumber=${page}&pageSize=${itemsPerPage}`);
                     this.loading = false
                 } catch (e) {
                     console.error(e)
                 }
            },
            showDialogRemoveProveedor(proveedorId)
            {
                this.deleteDialog = true;
                this.currentIdToBeDeleted = proveedorId;
            },
            hiddeDialogRemoveProveedor()
            {
                this.deleteDialog = false;
                this.currentIdToBeDeleted = 0;
            },
            async removeProveedor() {

                const result = await this.$api.delete(`/proveedores/${this.currentIdToBeDeleted}`);

                if (result) {
                    const itemIndex = this.proveedores.items.findIndex((item) => item.id === this.currentIdToBeDeleted);
                    this.proveedores.items.splice(itemIndex, 1)
                    this.hiddeDialogRemoveProveedor()
                }
            },
            editProveedor(proveedor) {
                this.$router.push({ name: 'EditFormProveedorView', params: { id: parseInt(proveedor.id) } });
            },
            registerProvider() {
                this.$router.push({ name: 'RegisterFormProveedorView' });
            },
            addFacturas(proveedorId) {
                this.$router.push({ name: 'AddInvoicesToProveedorView', params: { id: parseInt(proveedorId) } });
            },
            formatFechaAlta(value) {
                const date = parseISO(value)
                return format(date, 'dd/MM/yyyy HH:mm:ss')
            }
        },
    }
</script>

<style scoped>
    .v-data-table {
        width: 1138px;
        min-height: 370px;
    }
</style>