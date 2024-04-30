<template>
    <v-row>
        <v-col cols="12" class="text-center">
            <h2>Lista de Proveedores</h2>
        </v-col>
        <v-col cols="12" class="text-center">
            <v-btn class="text-none text-white"
                   color="success"
                   rounded="1"
                   variant="elevated"
                   @click="registerProvider">
                Registrar Proveedor
            </v-btn>
        </v-col>
    </v-row>
    <v-row align="center" justify="center">
        <v-col cols="auto">
            <v-table :hover="true" density="comfortable">
                <thead>
                    <tr>
                        <th class="text-left">
                            Nombre Proveedor
                        </th>
                        <th class="text-left">
                            RFC
                        </th>
                        <th class="text-left">
                            Fecha Alta
                        </th>
                        <th class="text-left">
                            Direccion
                        </th>
                        <th class="text-left">
                            Activo
                        </th>
                        <th class="text-left">
                            Facturas
                        </th>
                        <th class="text-left">

                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="proveedor in proveedores"
                        :key="proveedor.id">
                        <proveedor :proveedor="proveedor"
                                   @item-deleted="removeProveedor(proveedor.id)"
                                   @item-edited="editProveedor(proveedor)"
                                   @add-invoices="addFacturas(proveedor.id)">
                        </proveedor>
                    </tr>
                </tbody>
            </v-table>
        </v-col>
    </v-row>
</template>


<script>
    import Proveedor from './Proveedor.vue'

    export default {
        components: {
            Proveedor
        },
        data() {
            return {
                proveedores: []
            }
        },
        async mounted() {
            try {
                this.proveedores = await this.$api.get('/proveedores');
            } catch (e) {
                console.error(e)
            }
        },
        methods: {
            async removeProveedor(proveedorId) {
                const result = await this.$api.delete(`/proveedores/${proveedorId}`);

                if (result) {
                    const itemIndex = this.proveedores.findIndex((item) => item.id === proveedorId);
                    this.proveedores.splice(itemIndex, 1)

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
        },
    }
</script>