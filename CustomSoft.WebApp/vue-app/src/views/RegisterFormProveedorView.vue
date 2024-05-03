<template>
    <v-container>
        <h2>Registrar Proveedor</h2>
        <v-form v-model="form" @submit.prevent="updateProvider">
            <v-text-field v-model="provider.nombre"
                          :rules="[rules.required]"
                          label="Nombre del Proveedor"
                          required></v-text-field>

            <v-text-field v-model="provider.rfc"
                          :rules="[rules.required, rules.validateRfc]"
                          label="RFC"
                          required></v-text-field>

            <v-text-field v-model="provider.direccion"
                          :rules="[rules.required]"
                          label="Dirección"
                          required></v-text-field>

            <v-switch v-model="provider.activo"
                      label="Activo"></v-switch>

            <v-btn :disabled="!form" color="primary" type="submit">
                Guardar
            </v-btn>
            <v-btn class="mx-2" @click="cancelAction" color="primary" type="submit">
                Cancelar
            </v-btn>
        </v-form>
    </v-container>
</template>

<script>
    export default {
        data() {
            return {
                provider: {
                    nombre: '',
                    rfc: '',
                    direccion: '',
                    activo: false,
                },
                rules: {
                    required: value => !!value || 'Required.',
                    validateRfc: value => /^[A-Z&Ñ]{3,4}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[A-Z0-9]{2}[0-9A]$/.test(value) || 'RFC invalid format'
                },
                form: false,
            };
        },
        mounted() {
        },
        methods: {
            cancelAction() {
                this.$router.push({ name: 'home' });
            },
            async updateProvider() {
                const result = await this.$api.post(`/proveedores`, this.provider);

                if (result) {
                    this.$router.push({ name: 'home', params: {} });
                }
            }
        }
    };
</script>
