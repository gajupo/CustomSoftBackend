<template>
    <td>{{ proveedor.nombre }}</td>
    <td>{{ proveedor.rfc }}</td>
    <td>{{ formatFechaAlta }}</td>
    <td>{{ proveedor.direccion }}</td>
    <td>{{ getActiveAsString }}</td>
    <td>{{ proveedor.archivos.length }}</td>
    <td>
        <v-menu>
            <template v-slot:activator="{ props }">
                <v-btn color="primary"
                       v-bind="props">
                    Acciones
                </v-btn>
            </template>
            <v-list>
                <v-list-item>
                    <v-list-item-title @click="editProveedor">Editar</v-list-item-title>
                </v-list-item>
                <v-list-item>
                    <v-list-item-title @click="deleteProveedor">Eliminar</v-list-item-title>
                </v-list-item>
                <v-list-item>
                    <v-list-item-title @click="addInvoices">Cargar Facturas</v-list-item-title>
                </v-list-item>
            </v-list>
        </v-menu>
    </td>
</template>

<script>
    import { parseISO, format } from 'date-fns'
    export default {
        emits: ['item-deleted', 'item-edited'],
        props: {
            proveedor: {
                type: Object,
                default() {
                    return {}
                }
            }
        },
        computed: {
            getActiveAsString() {
                return this.proveedor.activo && this.proveedor.activo === true ? "Si" : "No" 
            },
            formatFechaAlta() {
                const date = parseISO(this.proveedor.fechaAlta)
                return format(date, 'dd/MM/yyyy HH:mm:ss')
            }
        },
        methods: {
            deleteProveedor() {
                this.$emit("item-deleted");
            },
            editProveedor() {
                this.$emit("item-edited");
            },
            addInvoices() {
                this.$emit("add-invoices");
            },
        }
    }
</script>

<style scoped>
    .v-list-item-title {
        cursor: pointer;
    }
</style>