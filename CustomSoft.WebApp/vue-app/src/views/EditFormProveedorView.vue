<template>
    <v-container>
        <v-form v-model="form" @submit.prevent="updateProvider">
            <v-text-field v-model="provider.nombre"
                          :rules="[rules.required]"
                          label="Nombre del Proveedor"></v-text-field>

            <v-text-field v-model="provider.rfc"
                          :rules="[rules.required, rules.validateRfc]"
                          label="RFC"></v-text-field>

            <v-text-field v-model="provider.direccion"
                           :rules="[rules.required]"
                          label="Dirección"></v-text-field>

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
        props: {
            id: Number
        },
        data() {
            return {
                provider: {
                    id: this.id,
                    nombre: '',
                    fechaAlta: '',
                    rfc: '',
                    direccion: '',
                    activo: false,
                    fechaCreacion: '',
                    archivos: []
                },
                rules: {
                    required: value => !!value || 'Required.',
                    validateRfc: value => /^[A-Z&Ñ]{3,4}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[A-Z0-9]{2}[0-9A]$/.test(value) || 'RFC invalid format'
                },
                form: false,
            };
        },
        async mounted() {
            await this.loadProvider();
        },
        methods: {
            cancelAction() {
                this.$router.push({ name: 'home' });
            },
            async loadProvider() {
                const result = await this.$api.get(`/proveedores/${this.provider.id}`);

                if (result) {
                    this.provider = result;
                }
            },
            async updateProvider() {
                // Here you would make an API call to update the provider
                console.log('Updating provider:', this.provider);

                const result = await this.$api.put(`/proveedores/${this.provider.id}`, this.provider);

                if (result) {
                    this.$router.push({ name: 'home', params: {} });
                }
            }
        }
    };
</script>
