<template>
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
                    <v-list-item-title id="btnDeleteProveedor" @click="deleteProveedor">Eliminar</v-list-item-title>
                </v-list-item>
                <v-list-item>
                    <v-list-item-title @click="addInvoices">Cargar Facturas</v-list-item-title>
                </v-list-item>
            </v-list>
        </v-menu>
</template>

<script>
    export default {
        emits: ['item-deleted', 'item-edited'],
        props: {
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