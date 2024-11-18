//-------------------------------------------------------------------------
// <copyright file="Recipe.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;

namespace Full_GRASP_And_SOLID
{
    public class Recipe : IRecipeContent // Modificado por DIP
    {
        // Cambiado por OCP
        private IList<BaseStep> steps = new List<BaseStep>();

        public Product FinalProduct { get; set; }
        
        public bool Cooked { get; private set; } = false;

        // Agregado por Creator
        public void AddStep(Product input, double quantity, Equipment equipment, int time)
        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        // Agregado por OCP y Creator
        public void AddStep(string description, int time)
        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        public void RemoveStep(BaseStep step)
        {
            this.steps.Remove(step);
        }

        // Agregado por SRP
        public string GetTextToPrint()
        {
            string result = $"Receta de {this.FinalProduct.Description}:\n";
            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetTextToPrint() + "\n";
            }

            // Agregado por Expert
            result = result + $"Costo de producción: {this.GetProductionCost()}";

            return result;
        }

        // Agregado por Expert
        public double GetProductionCost()
        {
            double result = 0;

            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetStepCost();
            }

            return result;
        }

        public int GetCoockTime()
        {
            int result = 0;

            foreach (BaseStep step in this.steps)
            {
                result += step.Time;
            }

            return result;
        }

        public void Cook()
        {
            if (this.Cooked)
            {
                return;
            }
            
            int cookTime = GetCoockTime();

            CountdownTimer timer = new CountdownTimer();

            Adapter adapter = new Adapter(this);
            
            timer.Register(cookTime, client: adapter );
        }
        
        public void MarkAsCooked()
        {
            this.Cooked = true;
        }
    }
}

/*
Las guías que fueron utilizadas: 

El de Expert, ya qué en GetCookTime() la responsabilidad de calcular el tiempo de cocción es asignada
a la clase que tiene la información de calcular el tiempo de cocción.

El de Creator, pues cree una instancia de CountdownTimer dentro de Cook() , quien tiene conocimiento de 
la clase Adapter.

Bajo Acoplamiento, el Adapter permite que la clase Cook no dependa directamente de la implementación del temporizador.

El de DIP, ya qué cree el método MarkAsCooked() en la clase Recipe, 
logrando que la clase Recipe no dependa de la clase Adapter. Y tenga una única responsabilidad.
Además, la responsabilidad de calcular el tiempo está separada en el método GetCookTime(). 
*/