using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedLINQ
{
	class Program
	{
		static Random rnd = new Random();
		static void Main(string[] args)
		{
			//LINQ();
			Closure();
		}
		static void Closure()
		{
			int[] arr = { 3, 6, 10, 4, 5, 1, 2 };
			var filteredArr = arr.Where(inBetween(3, 6)).ToArray();
			PrintCollection(filteredArr);

			Console.WriteLine();

			int[] arr2 = { 1, 4, 10 };
			var filteredArr2 = arr.Where(inArray(arr2)).ToArray();
			PrintCollection(filteredArr2);

			Console.WriteLine();
			Console.WriteLine(mySum(3)(4));
		}

		static void LINQ()
		{
			var products = new List<Product>();
			var brands = new List<Brand>()
			{
				new Brand() { Country = "Moldova", Name = "Bucuria"},
				new Brand() { Country = "Ukraine", Name = "Roshen"},
				new Brand() { Country = "USA", Name = "Nestle"}
			};

			for (int i = 0; i < 20; i++)
			{
				products.Add(new Product($"Product { i }", rnd.Next(1, 10), brands.ElementAt(rnd.Next(0, 3))));
			}

			Console.WriteLine("==== Products ====");
			PrintCollection(products);


			#region Filtering
			var result = products.Where(item => item.Calories <= 8 && item.Calories >= 4).ToList(); // do i need to call ToList() at the end?

			Console.WriteLine("==== Calories between 4 and 8 ====");
			PrintCollection(result);
			#endregion

			#region Projection
			var calories = products.Select(p => p.Calories).ToArray();

			Console.WriteLine("==== Calories ====");
			PrintCollection(calories);
			Console.WriteLine();
			#endregion

			#region Join
			// var insteadOfJoin = products.Select(p => new { Name = p.Name, BrandName = p.Brand.Name }); Why do we need to do a join???

			var join = brands.Join(products, b => b, p => p.Brand, (b, p) => new { BrandName = b.Name, ProductName = p.Name }).ToList();
			Console.WriteLine("==== Join brands to products ====");
			PrintCollection(join);
			#endregion

			#region Conversion methods
			ArrayList arrayList = new ArrayList() { 3, 5, "ab", "ed", 4.5f };
			List<string> strings = arrayList.OfType<string>().ToList(); // why does it need to be explicitly converted to list?
			Console.WriteLine("==== Return list of strings ====");
			PrintCollection(strings);

			#endregion

			#region Generation methods
			int[] arrOfInts = Enumerable.Range(0, 10).ToArray();
			Console.WriteLine("==== Generated array of 10 ints ====");
			PrintCollection(arrOfInts);
			Console.WriteLine();
			#endregion

			#region Ordering
			var orderedProducts = products.Where(item => item.Calories > 4)
										  .OrderBy(item => item.Calories).ToList();

			Console.WriteLine("==== Calories > 4, ordered ====");
			PrintCollection(orderedProducts);
			#endregion

			#region Grouping
			var productsGroupedByCalories = products.GroupBy(product => product.Calories);
			Console.WriteLine("==== Grouped by calories ====");
			foreach (var group in productsGroupedByCalories)
			{
				Console.WriteLine($"Calories: { group.Key }");
				foreach (var product in group)
				{
					Console.WriteLine($"\t{ product }");
				}
			}
			#endregion

			#region Quanitifers
			Product newProduct = new Product("Product 0", 4, brands.ElementAt(0));
			products[0] = newProduct;

			Console.WriteLine();
			Console.WriteLine($"Check if products list contains Product0 with 4 calories: { products.Contains(newProduct)}");



			Console.WriteLine();
			Console.WriteLine($"Check if calories of all products are > 0: { products.All(product => product.Calories > 0) }");



			Console.WriteLine();
			Console.WriteLine($"Check if calories of all products == 5 { products.All(product => product.Calories == 5) }");



			Console.WriteLine();
			Console.WriteLine($"Check if calories of any product == 5: { products.Any(product => product.Calories == 5) }");
			#endregion

			#region Aggregation
			var sumOfAllCalories = products.Sum(p => p.Calories);

			Console.WriteLine();
			Console.WriteLine($"Sum of all calories: { sumOfAllCalories }");



			//var minCalories = products.Min(p => p.Calories);
			var minCaloriesProducts = products.Where(p => p.Calories == products.Min(p => p.Calories));

			Console.WriteLine();
			Console.WriteLine("Min calories products: ");
			PrintCollection(minCaloriesProducts);
			#endregion

			#region Set operations
			int[] arr1 = new int[] { 1, 2, 3, 4 };
			int[] arr2 = new int[] { 2, 3, 4, 5 };

			Console.WriteLine();

			Console.WriteLine("Array1:");
			PrintCollection(arr1);

			Console.WriteLine("Array2:");
			PrintCollection(arr2);



			var union = arr1.Union(arr2).ToArray();

			Console.WriteLine($"Union of arr1 and arr2:");
			PrintCollection(union);



			var intersect = arr1.Intersect(arr2).ToArray();

			Console.WriteLine();
			Console.WriteLine($"Intersect of arr1 and arr2:");
			PrintCollection(intersect);



			var except = arr1.Except(arr2).ToArray();

			Console.WriteLine();
			Console.WriteLine($"arr1 - arr2:");
			PrintCollection(except);
			#endregion

			Console.WriteLine();
		}
		static void PrintCollection(IEnumerable collection)
		{
			foreach (var item in collection)
			{
				Console.WriteLine(item);
			}
			Console.WriteLine();
		}

		static void PrintCollection(int[] arr)
		{
			foreach (var item in arr)
			{
				Console.Write($"{item} ");
			}
			Console.WriteLine();
		}

		static Func<int, bool> inBetween(int x, int y)
		{
			return z => z > x && z < y;
		}
		
		static Func<int, bool> inArray(int[] arr)
		{
			return z => arr.Contains(z);
		}

		static Func<int, int> mySum(int x)
		{
			return y => y + x;
		}
	}

	class Product
	{
		public string Name { get; set; }
		public int Calories { get; set; }
		public Brand Brand { get; set; }
		public Product(string name, int calories, Brand brand)
		{
			Name = name;
			Calories = calories;
			Brand = brand;
		}

		public override string ToString()
		{
			return $"Name: {Name}, Calories: {Calories}, { Brand.ToString() }";
		}
	}

	class Brand
	{
		public string Name { get; set; }
		public string Country { get; set; }

		public override string ToString()
		{
			return $"Name: { Name }, Country: { Country }";
		}
	}
}