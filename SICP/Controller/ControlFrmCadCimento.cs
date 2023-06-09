﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SICP.Entidades.enums;
using SICP.Entidades;
using SICP.SubForms;
using SICP.Dados;

using System.Threading.Tasks;

namespace SICP.Controller
{
    class ControlFrmCadCimento
    {
        private FrmCadCimento _form;

        public ControlFrmCadCimento(FrmCadCimento WinForms)
        {
            _form = WinForms;
        }

        public void ControlRegistraMarcasCimento()
        {
            //CRIANDO OBJETO:
            this.ValidaCadastro();
            MatConstrucao mat = new MatConstrucao();
            mat.Cod = GenerateCodigo();
            mat.Descricao = _form.txt_NomeMarcaCimento.Text;
            mat.Custo = _form.NumUp_ValorCusto.Value;
            mat.ValLucro = mat.CalculaValorLucro(_form.NumUp_PerLucro.Value);
            mat.Tipo = TipoMaterial.CIMENTO;
            mat.QtdeEstoque = int.Parse(_form.NumUp_EstoqueInicial.Value.ToString());

            //INSERIR OBJETO NO BANCO DE DADOS:
            bool teste = ConexaoDAO.ValidateOperation(MaterialDAO.VerificaSeMatExist, mat);

            if (teste) throw new DomainsException("Erro: Material ja está cadastrado");
            ConexaoDAO.ModifyOperation(MaterialDAO.InsertNewMaterial, mat);
            System.Windows.Forms.MessageBox.Show("Material Inserido com sucesso !");
            this.LimpaCampos();
        }

        public void LimpaCampos()
        {
            _form.txt_NomeMarcaCimento.Text = "";
            _form.NumUp_EstoqueInicial.Value = 0;
            _form.NumUp_ValorCusto.Value = 0;
            _form.NumUp_PerLucro.Value = 0;
            _form.Lb_ValLucro.Text = "";
            _form.NumUp_ValorVenda.Value = 0;
        }

        public void ValidaCadastro()
        {
            if (_form.txt_NomeMarcaCimento.Text == string.Empty) throw new DomainsException("Campo de nome não pode ser vazio");
        }

        public void PreperadaControles()
        {
            //preenche combox Unidades
            for(int i = 0; i < 1000; i++)
            {
              //  _form.Cb_EstoqueInicial.Items.Add(i);
            }
        }

        public void ControlUpdateMarcaCimento()
        {
            ValidaCadastro();
            MatConstrucao mat = new MatConstrucao();
            mat.Cod = _form.lb_codigoMat.Text;
            mat.Descricao = _form.txt_NomeMarcaCimento.Text;
            mat.Custo = _form.NumUp_ValorCusto.Value;
            mat.ValLucro = mat.CalculaValorLucro(_form.NumUp_PerLucro.Value);
            mat.Tipo = TipoMaterial.CIMENTO;
            mat.QtdeEstoque = int.Parse(_form.NumUp_EstoqueInicial.Value.ToString());
            ConexaoDAO.ModifyOperation(MaterialDAO.UpdateInfoMat, mat);
            System.Windows.Forms.MessageBox.Show("Dados Atualizados com sucesso !");
        }


        public string GenerateCodigo()
        {
            Random rd = new Random();

            string codigo = "MC0000";
            bool teste = ConexaoDAO.ValidateOperation(MaterialDAO.VerificaSeCodeExist, codigo);

            while (teste)
            {
                int numeros = rd.Next();
                codigo = "MC" + numeros.ToString().Substring(0, 4);
                teste = ConexaoDAO.ValidateOperation(MaterialDAO.VerificaSeCodeExist, codigo);
            }
            return codigo;
        }
    }
}
