using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace Gen3EventLegality
{
	public static class Program
	{

		[Flags]
		public enum Algo : ulong
		{
			Unknown = 0x00000000,
			WSHMKR = 0x00000010,
			Offset = 0x00000020,
			ForceShiny = 0x00000040,
			RandOTG1 = 0x00000080, // X / 3 & 1
			RandOTG2 = 0x00000100, // X >> 3 & 1
			BACDPIDIV = 0x00000200,
			RandOTG3 = 0x00000400, // X >> 7 & 1
			WCEggs = 0x00000800,
			LimitRange = 0x00001000,
			PokeParkEggs = 0x00002000,
			DSPlay = 0x00004000,
			BerryGlitch = 0x00008000,
			NoIVs = 0x00010000,
			PCJP2003 = 0x00020000,
			CanBeShiny = 0x00040000,
			RandItem1 = 0x00080000, // X / 3 & 1
			RandItem2 = 0x00100000, // X >> 3 & 1
			RandItem3 = 0x00200000, // X >> 7 & 1
			MaleOTG = 0x00400000,
			FemaleOTG = 0x00800000,
			ItemFirst = 0x01000000, // OTG1/2 generally use this
			Stamp = 0x02000000, // Absol Item
			DFABPIDIV = 0x04000000,
			Box = 0x08000000,
			RandOTG4 = 0x10000000, // X >> E & 1 (rng6)
			BatchGen = 0x20000000, // Mystry Mew\
			ForceAntiShiny = 0x40000000,
			UnknownOTG = 0x80000000,
		}

		public static Dictionary<string, Algo> EventOptions = new Dictionary<string, Algo> {
			{ "5th Anniv Eggs", Algo.PCJP2003 | Algo.BACDPIDIV | Algo.Offset },
			{ "PCJP 2004/PCNY WISH Eggs", Algo.WCEggs },
			{ "PokePark 2005 Eggs", Algo.PokeParkEggs | Algo.CanBeShiny },
			{ "Pokemon Box Eggs", Algo.Box | Algo.BACDPIDIV },
			{ "E-Reader", Algo.DFABPIDIV | Algo.NoIVs | Algo.CanBeShiny },
			{ "PCNYabcd", Algo.ForceAntiShiny | Algo.UnknownOTG },
			{ "None", Algo.Unknown }
		};

		public static Dictionary<uint, Tuple<string, Algo>> EventsByID = new Dictionary<uint, Tuple<string, Algo>> {
			{ 30719, Tuple.Create("Wishing Star Jirachi", Algo.BACDPIDIV | Algo.Offset | Algo.ForceAntiShiny | Algo.RandItem1 | Algo.MaleOTG) },

			{ 20043, Tuple.Create("Wishmaker/METEOR Jirachi (20043)", Algo.BACDPIDIV | Algo.WSHMKR | Algo.RandItem1 | Algo.CanBeShiny) },

			{ 30317, Tuple.Create("English Berry Glitch Zigzagoon", Algo.BerryGlitch | Algo.BACDPIDIV | Algo.ForceShiny | Algo.LimitRange) },

			{ 21121, Tuple.Create("Japanese Berry Glitch Zigzagoon", Algo.BerryGlitch | Algo.BACDPIDIV | Algo.ForceShiny | Algo.LimitRange) },

			{ 30821, Tuple.Create("Stamp", Algo.BACDPIDIV | Algo.RandItem1 | Algo.RandOTG1) },

			{ 40707, Tuple.Create("2004 Tanabata Jirachi", Algo.BACDPIDIV | Algo.RandItem1 | Algo.FemaleOTG) },

			{ 41205, Tuple.Create("ANA Pikachu", Algo.BACDPIDIV | Algo.MaleOTG) },

			{ 50319, Tuple.Create("Yokohama Pikachu", Algo.BACDPIDIV | Algo.MaleOTG) },

			{ 50701, Tuple.Create("Sapporo Pikachu / Sunday Wobbuffet", Algo.BACDPIDIV | Algo.RandOTG2) },

			{ 50318, Tuple.Create("Pokepark Meowth", Algo.BACDPIDIV | Algo.MaleOTG) },

			{ 50716, Tuple.Create("Hado Mew", Algo.BACDPIDIV | Algo.RandOTG1) },

			{ 50707, Tuple.Create("2005 Tanabata Jirachi", Algo.BACDPIDIV | Algo.RandItem1 | Algo.FemaleOTG) },

			{ 50425, Tuple.Create("GW Pikachu", Algo.BACDPIDIV | Algo.RandOTG2) },

			{ 2005, Tuple.Create("Festa/ROCKS Metang", Algo.BACDPIDIV | Algo.MaleOTG) },

			{ 50901, Tuple.Create("Hado Titans", Algo.DSPlay | Algo.ItemFirst | Algo.BACDPIDIV | Algo.RandOTG4) },

			{ 60505, Tuple.Create("GCEA 6th Campaign", Algo.BACDPIDIV | Algo.RandOTG3) },

			{ 60321, Tuple.Create("GCEA 5th Campaign", Algo.BACDPIDIV | Algo.RandOTG3) },

			{ 60227, Tuple.Create("GCEA 4th Campaign", Algo.BACDPIDIV | Algo.RandOTG3) },

			{ 60114, Tuple.Create("GCEA 3rd Campaign", Algo.BACDPIDIV | Algo.RandOTG3) },

			{ 51224, Tuple.Create("GCEA 2nd Campaign", Algo.BACDPIDIV | Algo.RandOTG3) },

			{ 51126, Tuple.Create("GCEA 1st Campaign", Algo.BACDPIDIV | Algo.RandOTG3) },

			{ 60707, Tuple.Create("2006 Tanabata Jirachi", Algo.BACDPIDIV | Algo.RandOTG3 | Algo.RandItem3) },

			{ 60623, Tuple.Create("Pokepark Celebi", Algo.BACDPIDIV | Algo.RandOTG3) },

			{ 60510, Tuple.Create("Pokepark Mew", Algo.BACDPIDIV | Algo.RandOTG1) },

			{ 60720, Tuple.Create("Misturin Celebi", Algo.BACDPIDIV | Algo.RandOTG3) },

			{ 60731, Tuple.Create("Pokepark Jirachi (1st Distribution)", Algo.BACDPIDIV | Algo.RandOTG1) },

			{ 60830, Tuple.Create("Pokepark Jirachi (2nd Distribution)", Algo.BACDPIDIV | Algo.RandOTG1) },

			{ 28606, Tuple.Create("DOEL Doexys", Algo.BACDPIDIV | Algo.RandOTG3) },

			{ 10, Tuple.Create("JAA & SPACE C", Algo.BACDPIDIV | Algo.RandOTG3) },

			{ 6808, Tuple.Create("Bryant Park", Algo.BACDPIDIV | Algo.RandOTG3) },

			{ 6227, Tuple.Create("Top 10", Algo.BACDPIDIV | Algo.RandOTG3) },

			{ 20078, Tuple.Create("Aura Mew", Algo.BACDPIDIV | Algo.RandOTG3) },

			{ 31121, Tuple.Create("Ageto Celebi, Colos Pikachu", Algo.DFABPIDIV) },

			{ 10048, Tuple.Create("Mattle Ho-Oh", Algo.DFABPIDIV) },

			{ 6930, Tuple.Create("Mystery Mew", Algo.BACDPIDIV | Algo.BatchGen | Algo.RandOTG1) }
		};

		public static uint[] MystryMewSeeds =
		{
			0x5189E6C6, 0x00005E2B, 0x5ADFDAC6, 0x83A723B5, 0x8AF5BCA8,
			0xA587868F, 0xE8D1A0BF, 0x0000E6CC, 0x39AEB123, 0xF6B75FDE,
			0xFF889A6D, 0x0ACAC680, 0x00005841, 0xD30AA204, 0x65E00D7B,
			0x3BCB20D6, 0x077B5585, 0x000020BF, 0x849ED80A, 0xE1289CE9,
			0x329A3E0C, 0x08027B63, 0x0000E95D, 0xF29915B0, 0x9E85FD77,
			0xA8502FE2, 0xC9F74B61, 0x000034F3, 0xFB06486E, 0xAA4E84BD,
			0x322A1B90, 0xDF9B63D7, 0x00005327, 0xBE401AD2, 0x8F25C691,
			0x464C6B14, 0x8029764B, 0x00009D9C, 0x0DD7C0B3, 0x0A70952E,
			0xC0F8CE7D, 0x1F3BAE50, 0x0000643F, 0x8B2AED8A, 0xDC15BC69,
			0xFD76BF8C, 0x547956E3, 0x00005A60, 0x9E22DB67, 0x930C9212,
			0x84A830D1, 0x0681BC54, 0x0000C2D4, 0xE8905F0B, 0x724F1026,
			0x1189DB95, 0x61928D08, 0x0000D1E4, 0x15E075DB, 0x0D9EFFB6,
			0x688670E5, 0x7D10A118, 0x000078D2, 0x726C2C91, 0x60C2B914,
			0x224A0C4B, 0x21875866, 0x0000A443, 0x8BBF4E7E, 0xB029768D,
			0x2F283A20, 0x09512C27, 0x00001C09, 0x7AD488AC, 0x71834383,
			0x3BE940BE, 0x908A2FCD, 0x0000C385, 0xD3135138, 0xC4EC6CDF,
			0x2675FBAA, 0x3B111A09, 0x0000FE9D, 0x15D825F0, 0xB95EDCB7,
			0x34076222, 0xF50044A1, 0x000046CE, 0x43CB219D, 0x19DA7CF0,
			0x0D6597B7, 0xB939F122, 0x0000302D, 0x37277540, 0x48E342C7,
			0x835833F2, 0x7A14C331, 0x00009C40, 0xBE998DC7, 0xFFE012F2,
			0x9C2CE631, 0x2583F434, 0x000095A0, 0xB21AF9A7, 0xE3EDC752,
			0x5E8BE111, 0x9394BB94, 0x00004B63, 0xEDCB211E, 0xB6D7A6AD,
			0x6852B1C0, 0xB5B4AD47, 0x00007667, 0xB3728112, 0x372A23D1,
			0xEED0A354, 0xF7BCDD8B, 0x0000EE7F, 0x646DDECA, 0xEE2338A9,
			0xF9DF5ACC, 0x48505523, 0x000050AB, 0x5990A346, 0x84AAEA35,
			0x2F300928, 0xC96FC10F, 0x00000C13, 0x9A9D8B0E, 0x02B664DD,
			0xCE138330, 0xB6FE14F7, 0x00008A92, 0xE12E6751, 0x9850B8D4,
			0x66CDFD0B, 0x396DB626, 0x00009E86, 0x6F43C875, 0x9A5EFE68,
			0x85DA314F, 0x58CD4A5A, 0x0000C953, 0x9544A34E, 0xBB992C1D,
			0xB366FD70, 0x78843637, 0x0000508E, 0xA84B745D, 0x6E7434B0,
			0x41746077, 0xC9F5C6E2, 0x00009690, 0x994EB2D7, 0x0E8667C2,
			0x28799BC1, 0x866BB784, 0x000056CC, 0x810C6123, 0x018A4FDE,
			0xD74BCA6D, 0x89CD3680, 0x0000CD96, 0xC59B7F45, 0xC1800DF8,
			0x1EAB669F, 0x90F57E6A, 0x0000AFFB, 0xD5C5D956, 0xC49D4C05,
			0x0B43D7B8, 0xE4D0795F, 0x000067A3, 0x7A03DC5E, 0x984644ED,
			0x82706700, 0x0702DD87, 0x0000C6CE, 0xE7E9A19D, 0x491EFCF0,
			0x14B817B7, 0x2BE27122, 0x0000E62C, 0xF4E18B03, 0x84FC4A3E,
			0xCE3F734D, 0xBE3B73E0, 0x00006E62, 0x0F0A2FE1, 0xA8A80324,
			0x04D1321B, 0xD62DDAF6, 0x0000B1F2, 0xCFAAC931, 0x955B0B34,
			0x87CE9DEB, 0x10B30B86, 0x00000652, 0xCF9CE411, 0x307A7294,
			0x76BD3FCB, 0x8AF69DE6, 0x0000CC43, 0xBF08D67E, 0x1EEEDE8D,
			0x81720220, 0x2D25D427, 0x000093D0, 0x9DC80B17, 0x80826F02,
			0x82D15601, 0x9BF518C4, 0x0000A8AC, 0xB606E383, 0x344660BE,
			0x0FBACFCD, 0xB7247E60, 0x0000EE9F, 0xD896E66A, 0xD1AF09C9,
			0x6A212F6C, 0x246CFF43, 0x00008B48, 0x65D7C2AF, 0x967BEE3A,
			0x55446659, 0x2D50BDBC, 0x00000EEE, 0x7026D13D, 0xC8B85610,
			0x0A735457, 0x632CE342, 0x0000B831, 0x940CFE34, 0x178D84EB,
			0x49281686, 0x6F6B6075, 0x00005BC1, 0x87877784, 0x492AECFB,
			0x82FE6256, 0x1A9DF105, 0x0000941D, 0x7D4AC570, 0x6C0ADE37,
			0x98EFBDA2, 0x659A1221, 0x0000CD47, 0x0ED63472, 0xF7BEE1B1,
			0xB188E1B4, 0x898EBA6B, 0x0000EBB2, 0x71B68BF1, 0x3BAE72F4,
			0x1E9C56AB, 0x4C6E1146, 0x00004A0D, 0x24549FA0, 0x46F15BA7,
			0x48932152, 0x75425311, 0x00009C37, 0x75EE03A2, 0x97E0C021,
			0x149A6264, 0x8F9BE45B, 0x0000F0E4, 0x9D9FD8DB, 0x7D1196B6,
			0x284C6BE5, 0xB8D57018, 0x00001614, 0xD311354B, 0x0D841D66,
			0x0E2803D5, 0x1ED42448, 0x0000E90A, 0x8D4EA9E9, 0x1853570C,
			0x8D257063, 0x470CF21E, 0x0000BE96, 0x525FEC45, 0x17F606F8,
			0x7FCFBB9F, 0x6385BF6A, 0x00002389, 0xB472522C, 0xC6484703,
			0x8B57163E, 0x27420F4D, 0x0000AC08, 0x47D5906F, 0x7793F4FA,
			0x38A40219, 0x72A6DA7C, 0x000056BA, 0xEFB54CD9, 0x318BAA3C,
			0xC826C2D3, 0x5DFD16CE, 0x00005240, 0x1FB7EBC7, 0xCC8C78F2,
			0x03F13431, 0x233A8A34, 0x00006E06, 0x611439F5, 0xBA7609E8,
			0x6213EECF, 0xBD5B71DA, 0x000045F3, 0xB0D2556E, 0x8C959DBD,
			0xD5231090, 0xCED5C4D7, 0x00006944, 0x4BBA87BB, 0x157C4216,
			0x614A41C5, 0xD3586678, 0x0000C962, 0x63B7DEE1, 0xBE42B624,
			0x4005D91B, 0xC7B1A5F6, 0x0000DFED, 0x307F5600, 0x02C9D087,
			0x0CE5FAB2, 0x5F831EF1, 0x00002939, 0xD9BD2D1C, 0xB7A1F233,
			0xFF0A60AE, 0xC65A4BFD, 0x0000EFC8, 0x36B3B52F, 0x3998B6BA,
			0xE4042CD9, 0x77570A3C, 0x00006457, 0xA249B342, 0x46BE9941,
			0x11A81F04, 0x398ED67B, 0x00009DE4, 0x533411DB, 0x4A6B2BB6,
			0xFD8CECE5, 0xCE7C2D18, 0x00004C79, 0xCEEF935C, 0x5FA64F73,
			0xA78E98EE, 0x3DEDB33D, 0x000013C9, 0x8840916C, 0xC7775943,
			0x4AF06F7E, 0xD126538D, 0x0000A153, 0x61FB1B4E, 0x4CD3C41D,
			0x611D3570, 0x54AF8E37, 0x0000E991, 0xCF5BC214, 0x3049314B,
			0x7EBB2966, 0x0FA5DFD5, 0x000077EF, 0x82691E7A, 0x5A9AE599,
			0x8DF70FFC, 0x401C0193, 0x0000C92C, 0xAFB2A203, 0x4E46C53E,
			0x1296C24D, 0x444146E0, 0x00000D43, 0x6A23537E, 0xF466A78D,
			0x7884E720, 0x620E6527, 0x00008655, 0x369F3CC8, 0x2D140E2F,
			0x0095EBBA, 0xDA41CDD9, 0x0000FE4E, 0x5F12DB1D, 0xEF9DB070,
			0xC6E4DD37, 0xED9980A2, 0x0000306E, 0x0B1ACCBD, 0xFF174390,
			0x8F5DEBD7, 0xD12EFCC2, 0x00001263, 0x04B58C1E, 0x65CB25AD,
			0x730FF4C0, 0xA2A5A447, 0x00001EA5, 0x53AD07D8, 0x5F89F0FF,
			0xC347774A, 0x23608F29, 0x0000967D, 0x1C565650, 0x4F6B6397,
			0xEDD28582, 0xBBBAB281, 0x00005EF3, 0x70E04A6E, 0x51D0FEBD,
			0x88912D90, 0xC53AADD7, 0x00000932, 0x3F4C9371, 0xA20A3C74,
			0x8CA75A2B
		};

		public static void Main(string[] args)
		{
			Console.WriteLine("If an egg event or PCNY, press enter");
			Console.Write("TID:");
			uint[] numbers = GetNumbers();
			bool gotTID = numbers.Length > 0;
			uint TID = gotTID ? numbers[0] : 9999;
			
			bool foundEvent = EventsByID.TryGetValue(TID, out var option);

			if (!foundEvent)
			{
				if (!gotTID)
					Console.WriteLine("Could not find event by TID. Maybe it is an egg event or PCNY?");

				int index;
				Dictionary<string, Algo>.KeyCollection keys = EventOptions.Keys;
				do
				{
					int count = 0;
					foreach (string eventName in keys)
						Console.WriteLine("{0}. {1}", (++count).ToString().PadLeft(2), eventName);

					Console.Write("----------\nChoose an event by number:");
				} while (!int.TryParse(Console.ReadLine(), out index) || index < 0 || index > EventOptions.Keys.Count);

				KeyValuePair<string, Algo> choice = EventOptions.ElementAt(index - 1);

				if (choice.Value == Algo.Unknown)
					return;

				option = Tuple.Create(choice.Key, choice.Value);
			}

			Algo algo = option.Item2;

			bool forcedShiny = Has(algo, Algo.ForceShiny);
			bool WCEggs = Has(algo, Algo.WCEggs);
			bool hasOffset = Has(algo, Algo.Offset);
			bool berryGlitch = Has(algo, Algo.BerryGlitch);
			bool XDAlgo = Has(algo, Algo.DFABPIDIV);
			bool Box = Has(algo, Algo.Box);
			bool mystryMew = TID == 6930;
			bool batchGen = Has(algo, Algo.BatchGen);
			bool noIVs = Has(algo, Algo.NoIVs);
			bool forceAntishiny = Has(algo, Algo.ForceAntiShiny);

			List<Tuple<uint, uint, uint>> PIDTIDs = new List<Tuple<uint, uint, uint>>();
			bool finished;
			
			/*
				if (!gotTID && forceAntishiny) {
					Console.Write("TID:");
					uint fTid;
					GetNumbers(out fTid);
					TID = fTid;
				}
			*/
			
			do
			{
				Console.Write("PID <TID> <SID>:");
				numbers = GetNumbers();
				finished = numbers.Length == 0;
				if (!finished) PIDTIDs.Add(new Tuple<uint, uint, uint>(numbers[0], numbers.Length > 1 ? numbers[1] : TID, numbers.Length > 2 ? numbers[2] : 0));
			} while (!finished);

			Console.WriteLine("----------");

			uint seedRange = Has(algo, Algo.LimitRange) ? 0x100u : 0x10000u;

			for (int p = 0; p < PIDTIDs.Count; p++)
			{
				uint PID = PIDTIDs[p].Item1;
				TID = PIDTIDs[p].Item2;
				uint SID = PIDTIDs[p].Item3;

				bool found = false;

				uint rand1;
				uint rand2;
				uint rand3;
				uint rand4;
				uint rand5;
				if (XDAlgo)
				{
					bool another = false;

					// XD - Do it backwards - DFAB
					for (uint i = 0; XDAlgo && i < seedRange; i++)
					{
						uint pid;
						var seed = ((PID & 0xFFFF) << 0x10) | i;

						do
						{
							rand1 = seed;
							rand2 = Prev(rand1, algo);
							rand3 = Prev(rand2, algo);

							pid = ((rand2 >> 0x10) << 0x10) | (rand1 >> 0x10);

							seed = rand3;

						} while (!noIVs && isShiny(pid, TID, SID));

						if (pid == PID)
						{
							if (another)
								Console.WriteLine(" - ");

							found = true;

							rand4 = Prev(rand3, algo); // IV2
							rand5 = Prev(rand4, algo); // IV1

							Console.WriteLine("Found Seed: {0:X8} ({1:X8}) - {2}{3}", Prev(Prev(rand5, algo), algo), PID, option.Item1, noIVs ? " " + eReaderType(pid) : "");

							Console.WriteLine("Nature: {0}", index2Nature(PID));

							if (Has(algo, Algo.CanBeShiny))
								Console.WriteLine("Shiny: Can be shiny");
							else
								Console.WriteLine("Shiny: Cannot be shiny");

							uint[] ivs;

							if (noIVs)
								ivs = new uint[] { 0, 0, 0, 0, 0, 0 };
							else
								ivs = ParseStats((rand5 >> 0x10) & 0x7FFF, (rand4 >> 0x10) & 0x7FFF);

							Console.WriteLine("IVs: {0}, {1}, {2}, {4}, {5}, {3}", ivs[0], ivs[1], ivs[2], ivs[3], ivs[4], ivs[5]);

							if (TID == 31121) // Ageto/Colos
								Console.WriteLine("OTG: Female (Ageto) - Male (Colos)");

							another = true;
						}
					}

					Console.WriteLine("----------");
				}

				if (WCEggs || Has(algo, Algo.PokeParkEggs))
				{
					bool another = false;

					// Do it backwards, find a ABDE
					for (uint i = 0; i < seedRange; i++)
					{
						rand1 = (((PID & 0xFFFF) << 0x10)) | i; // PIDL
						rand2 = Next(rand1, algo); // PIDH

						uint pid = ((rand2 >> 0x10) << 0x10) | (rand1 >> 0x10);

						if (pid == PID)
						{
							if (another)
								Console.WriteLine(" - ");

							found = true;
							Console.WriteLine("Found Wondercard Seed: {0:X8} ({1:X8}) - {2}", Prev(rand1, algo), PID, option.Item1);

							Console.WriteLine("Nature: {0}", index2Nature(PID % 25));

							Console.WriteLine("Shiny: Can be shiny");

							rand3 = Next(rand2, algo); // forme (unown), not used.
							rand4 = Next(rand3, algo);
							rand5 = Next(rand4, algo);

							uint[] ivs = ParseStats((rand4 >> 0x10) & 0x7FFF, (rand5 >> 0x10) & 0x7FFF);
							Console.WriteLine("IVs: {0}, {1}, {2}, {4}, {5}, {3}", ivs[0], ivs[1], ivs[2], ivs[3], ivs[4], ivs[5]);

							another = true;
						}
					}

					if (WCEggs)
						Console.WriteLine("----------");
				}

				if (Box)
				{
					bool another = false;

					// Do it backwards, find a BACD
					for (uint i = 0; i < seedRange; i++)
					{
						rand1 = (PID & 0xFFFF0000) | i;
						rand2 = Next(rand1, algo);

						uint pid = ((rand1 >> 0x10) << 0x10) | (rand2 >> 0x10);

						if (pid == PID)
						{
							if (another)
								Console.WriteLine(" - ");

							found = true;
							Console.WriteLine("Found Seed: {0:X8} ({1:X8}) - {2}", Prev(rand1, algo), PID, option.Item1);

							Console.WriteLine("Nature: {0}", index2Nature(PID % 25));

							Console.WriteLine("Shiny: Can be shiny");

							rand3 = Next(rand2, algo);
							rand4 = Next(rand3, algo);

							uint[] ivs = ParseStats((rand3 >> 0x10) & 0x7FFF, (rand4 >> 0x10) & 0x7FFF);
							Console.WriteLine("IVs: {0}, {1}, {2}, {4}, {5}, {3}", ivs[0], ivs[1], ivs[2], ivs[3], ivs[4], ivs[5]);

							another = true;
						}
					}

					Console.WriteLine("----------");
				}

				uint rand6;
				if (forceAntishiny)
				{
					bool another = false;

					for (uint i = 0; i < seedRange; i++)
					{
						rand2 = ((PID & 0xFFFF) << 0x10) | i;
						rand1 = Prev(rand2, algo);
						rand3 = Next(rand2, algo);
						rand4 = Next(rand3, algo);
						rand5 = Next(rand4, algo);

						if (((rand2 >> 0x10) ^ TID ^ SID ^ (rand1 >> 0x10)) == (PID >> 0x10) && !isShiny(PID, TID, SID))
						{
							if (another)
								Console.WriteLine(" - ");

							uint seed = Prev(rand1, algo);

							found = true;
							Console.WriteLine("Found Seed: {0:X8} ({1:X8}) - TID {2}", seed, PID, TID);

							Console.WriteLine("Nature: {0}", index2Nature(PID % 25));

							Console.WriteLine("Shiny: Cannot be shiny");

							uint[] ivs = ParseStats((rand3 >> 0x10) & 0x7FFF, (rand4 >> 0x10) & 0x7FFF);
							Console.WriteLine("IVs: {0}, {1}, {2}, {4}, {5}, {3}", ivs[0], ivs[1], ivs[2], ivs[3], ivs[4], ivs[5]);

							uint d = Prev(rand1, algo);
							uint c = Prev(d, algo);
							uint b = Prev(c, algo);

							another = true;
						}
					}

					Console.WriteLine("----------");
				}

				if (!WCEggs && !Box && (Has(algo, Algo.BACDPIDIV) || Has(algo, Algo.PokeParkEggs)))
				{
					for (uint i = 0; i < seedRange; i++)
					{
						uint rand1a;
						uint rand1b;
						uint seed = rand1a = rand1b = i;
						uint originalSeed = seed;
						Tuple<string, string, uint, bool> entry = null;
						uint TSV = 0;

						if (hasOffset)
						{
							rand1a = Next(seed, algo);
							rand1b = Next(rand1a, algo);

							uint tempPid = (rand1a & 0xFFFF0000) | (rand1b >> 0x10);

							/* This never actually happens in seed range 0->FFFF
										if (tempPid == 0)
										tempPid |= 0x20030000;

										if ((tempPid & 0xFFFF) == 0)
										tempPid |= 0x327;
							*/
							
							entry = GetEggEntry(tempPid, seed);

							forcedShiny = entry.Item4;
						}

						uint originalPID, pid, otgRand, itemRand, batchCount = 1;

						do
						{
							rand1 = Next(rand1b, algo); // PIDH
							rand2 = Next(rand1, algo); // PIDL

							pid = ((rand1 >> 0x10) << 0x10) | (rand2 >> 0x10);

							originalPID = pid;

							if (mystryMew)
								while (isShiny(pid, TID, SID))
								{
									rand1 = Next(rand2, algo); // PIDH
									rand2 = Next(rand1, algo); // PIDL

									pid = ((rand1 >> 0x10) << 0x10) | (rand2 >> 0x10);
								}

							if (Has(algo, Algo.PCJP2003) && PID - pid < 8)
							{
								TSV = getTSVFromPID(pid);
								pid = PID;
							}

							uint jpid;
							uint epid;

							if (forcedShiny)
							{
								rand2 = Next(rand2, algo);

								if (berryGlitch)
								{
									epid = forceShinyPID(rand1 >> 0x10, rand2 >> 0x10, 30317, SID);
									jpid = forceShinyPID(rand1 >> 0x10, rand2 >> 0x10, 21121, SID);

									pid = epid == PID ? epid : jpid;
								}
								else
								{
									TSV = getTSVFromForceShiny(PID);
									pid = forceShinyPID(rand1 >> 0x10, rand2 >> 0x10, TSV, SID);
									originalPID = pid;
								}
							}

							rand3 = Next(rand2, algo); // IV1
							rand4 = Next(rand3, algo); // IV2
							rand5 = Next(rand4, algo); // Item if available or OTG if Item not available
							rand6 = Next(rand5, algo); // OTG if Item available
							Next(rand6, algo);

							itemRand = Has(algo, Algo.ItemFirst) ? rand5 : rand6;
							otgRand = !Has(algo, Algo.ItemFirst) ? rand5 : rand6;

							if (pid != PID)
								// in case of antishiny, we have to go back.
								rand1b = Next(Next(Next(Next(Next(rand1b, algo), algo), algo), algo), algo); // PID, IV, OTG -> next in batch.

							batchCount++;

						} while (batchGen && pid != PID && batchCount <= 5);

						if (batchCount > 6 || pid != PID)
							continue;

						found = true;
						string knownSeed = "Found Seed";

						if (berryGlitch)
							knownSeed = originalSeed >= 3 && originalSeed <= 180 ? "Known Seed" : "Unknown Seed";

						string mystryMewBatch = "";

						if (mystryMew)
						{
							knownSeed = MystryMewSeeds.Contains(rand1b) ? "Known Seed" : "Unknown Seed";

							if (knownSeed == "Known Seed")
							{
								long mewIndex = Array.IndexOf(MystryMewSeeds, rand1b) + 1;
								mystryMewBatch = string.Format("{0}/{1} {2}", mewIndex, MystryMewSeeds.Length, string.Format("{0} Slot {1}", mewIndex < 7 ? "Party" : "PC", mewIndex < 7 ? mewIndex : mewIndex - 6));
							}
							else
								mystryMewBatch = string.Format(" ({0} of 5)", batchCount);
						}

						Console.WriteLine("{0}: {1:X8} - {2} {3}", knownSeed, i, option.Item1, mystryMewBatch);
						Console.WriteLine("PID: {0:X8} ({1})", PID, index2Nature(PID % 25));

						uint rtc = seed ^ (seed < 0x5A0u ? 0x10001u : 0x0u);

						if (hasOffset)
						{
							uint days = rtc / 1440;
							uint hours = rtc % 1440 / 60;
							uint mins = rtc % 60;
							Console.WriteLine("RTC: {0} day{1}, {2} hour{3}, {4} minute{5}", days, days != 0 ? "s" : "", hours, hours != 0 ? "s" : "", mins, mins != 0 ? "s" : "");

							if (Has(algo, Algo.PCJP2003))
							{
								Console.WriteLine("Species: {1}{0} {2}", entry.Item1, entry.Item4 ? "Forced Shiny " : pid - originalPID > 0 && pid - originalPID < 8 ? "Anti-Shiny " : "", entry.Item2);
								Console.WriteLine("TSV: {0:X4}", TSV);
							}
						}

						if (Has(algo, Algo.PCJP2003) && !forcedShiny)
							Console.WriteLine("Shiny: Cannot be shiny if hatched by original trainer");
						else if (Has(algo, Algo.CanBeShiny))
							Console.WriteLine("Shiny: Can be shiny");
						else if (forcedShiny)
							Console.WriteLine("Shiny: Must be shiny");
						else
							Console.WriteLine("Shiny: Cannot be shiny");

						uint[] ivs;

						if (Has(algo, Algo.NoIVs))
							ivs = new uint[] { 0, 0, 0, 0, 0, 0 };
						else
							ivs = ParseStats((rand3 >> 0x10) & 0x7FFF, (rand4 >> 0x10) & 0x7FFF);

						Console.WriteLine("IVs: {0}, {1}, {2}, {4}, {5}, {3}", ivs[0], ivs[1], ivs[2], ivs[3], ivs[4], ivs[5]);

						if (Has(algo, Algo.BerryGlitch))
							Console.WriteLine("OTG: {0}", ((otgRand >> 0x10) / 3 & 1) == 1 ? "Female (RUBY)" : "Male (SAPHIRE)");
						else if (Has(algo, Algo.RandOTG3))
							Console.WriteLine("OTG: {0}", ((otgRand >> 0x17) / 1 & 1) == 0 ? "Female" : "Male");
						else if (Has(algo, Algo.RandOTG2))
						{
							if (TID == 50701)
							{
								Console.WriteLine("Sunday Wob OTG: {0}", ((otgRand >> 0x13) / 1 & 1) == 1 ? "Female" : "Male");
								Console.WriteLine("Sapporo Pikachu OTG: Male (Always)");
							}
							else
								Console.WriteLine("OTG: {0}", ((otgRand >> 0x13) / 1 & 1) == 1 ? "Female" : "Male");
						}
						else if (Has(algo, Algo.RandOTG1))
							Console.WriteLine("OTG: {0}", ((otgRand >> 0x10) / 3 & 1) == 1 ? "Female" : "Male");
						else if (Has(algo, Algo.MaleOTG))
							Console.WriteLine("OTG: Male (Always)");
						else if (Has(algo, Algo.FemaleOTG))
							Console.WriteLine("OTG: Female (Always)");

						if (Has(algo, Algo.RandItem3))
							Console.WriteLine("Item: {0}", ((itemRand >> 0x17) & 1) == 0 ? "Salac Berry" : "Ganlon Berry");
						else if (Has(algo, Algo.RandItem2))
							Console.WriteLine("Item: {0}", ((itemRand >> 0x13) & 1) == 0 ? "Salac Berry" : "Ganlon Berry");
						else if (Has(algo, Algo.RandItem1))
						{
							if (Has(algo, Algo.WSHMKR))
								Console.WriteLine("Item: {0}", ((itemRand >> 0x10) / 3 & 1) == 0 ? "Salac Berry" : "Ganlon Berry");
							else if (hasOffset)
							{
								string berry =
									$"Ganlon Berry ({((rtc & 0x1) == 0 ? "99.8%" : "0.2%")}), Salac Berry ({((rtc & 0x1) == 1 ? "99.8%" : "0.2%")})";

								Console.WriteLine("Item: {0}", berry);
							}
							else
								Console.WriteLine("Item: {0}", ((itemRand >> 0x10) / 3 & 1) == 0 ? "Petaya Berry" : "Apicot Berry");
						}
					}

					if (!found)
						Console.WriteLine("No seeds found for {0:X8}", PID);

					Console.WriteLine("----------");
				}
			}

			Console.WriteLine("Complete. Press enter to exit.");
			Console.ReadLine();
		}

		public static bool isShiny(uint pid, uint tid, uint sid) => ((pid >> 16) ^ (pid & 0xFFFF) ^ tid ^ sid) < 8;

		public static uint forceShinyPID(uint seed, uint shiny, uint tid, uint sid) => (seed << 0x10) | ((seed ^ tid ^ sid) & 0xFFF8) | (shiny & 7);

		public static uint getTSVFromForceShiny(uint pid) => (pid >> 0x10) ^ (pid & 0xFFF8);

		public static uint getTSVFromPID(uint pid) => (pid >> 0x10) ^ (pid & 0xFFFF);

		public static bool Has(Algo algo, Algo option) => (algo & option) != 0;

		public static uint[] GetNumbers()
		{
			string[] lines = Console.ReadLine()?.Trim().Split(' ');

			if (string.IsNullOrEmpty(lines?[0])) return new uint[]{};

			List<uint> numbers = new List<uint>();

			foreach (string str in lines)
			{
				uint number;
				bool success = str.StartsWith("0x") ?
					uint.TryParse(str.Replace("0x", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number) :
					uint.TryParse(str, out number);

				if (!success)
					return new uint[] { };
				
				numbers.Add(number);
			}

			return numbers.ToArray();
		}

		public static uint GetRandomEntry(uint randVal, uint max)
		{
			uint high = randVal >> 16;
			uint first = ((high << 2) & 0xFFFF) + high;
			uint second = ((randVal & 0xFFFF) << 1) + (first >> 16);

			second += high + (second >> 16);

			return (max * (second & 0xFFFF)) >> 16;
		}

		public static Tuple<string, string, uint, bool> GetEggEntry(uint randVal, uint seed)
		{
			uint result = GetRandomEntry(randVal, 1000);
			int count = 0;

			while (true)
			{
				if (result < PCJP2003Types[count].Item3)
					return PCJP2003Types[count];

				result -= PCJP2003Types[count++].Item3;
			}
		}

		public static Tuple<string, string, uint, bool>[] PCJP2003Types = new Tuple<string, string, uint, bool>[]
		{
			new Tuple<string,string,uint,bool>("Pichu","Teeter Dance",100, false),
			new Tuple<string,string,uint,bool>("Pichu","Teeter Dance",25, true),
			new Tuple<string,string,uint,bool>("Pichu","Wish",100, false),
			new Tuple<string,string,uint,bool>("Pichu","Wish",25, true),
			new Tuple<string,string,uint,bool>("Bagon","Iron Defense",125, false),
			new Tuple<string,string,uint,bool>("Bagon","Wish",125, false),
			new Tuple<string,string,uint,bool>("Absol","Spite",125, false),
			new Tuple<string,string,uint,bool>("Absol","Wish",125, false),
			new Tuple<string,string,uint,bool>("Ralts","Charm",125, false),
			new Tuple<string,string,uint,bool>("Ralts","Wish",125, false),
		};

		public static uint Prev(uint seed, Algo algo) =>
			Has(algo, Algo.BACDPIDIV) ||
			Has(algo, Algo.PokeParkEggs) ||
			Has(algo, Algo.WCEggs) ||
			Has(algo, Algo.ForceAntiShiny) ? PrevGBA(seed) : PrevXD(seed);

		public static uint Next(uint seed, Algo algo) =>
			Has(algo, Algo.BACDPIDIV) ||
			Has(algo, Algo.PokeParkEggs) ||
			Has(algo, Algo.WCEggs) ||
			Has(algo, Algo.ForceAntiShiny) ? NextGBA(seed) : NextXD(seed);

		public static uint PrevXD(uint seed) => seed * 0xB9B33155u + 0xA170F641u;

		public static uint NextXD(uint seed) => seed * 0x343FDu + 0x269EC3u;

		public static uint PrevGBA(uint seed) => seed * 0xEEB9EB65u + 0xA3561A1u;

		public static uint NextGBA(uint seed) => seed * 0x41C64E6Du + 0x00006073u;

		public static string eReaderType(uint pid)
		{
			uint nature = pid % 25;

			if (nature == 16 && pid % 256 < 128) // Mareep
				return "Mareep";
			if (nature == 11 && pid % 256 >= 128) // Scizor
				return "Scizor";
			if (nature == 22 && pid % 256 < 32) // Togepi
				return "Togepi";

			return "HACKED";
		}

		public static string index2Nature(uint nature)
		{
			return nature switch
			{
				0 => "Hardy",
				1 => "Lonely",
				2 => "Brave",
				3 => "Adamant",
				4 => "Naughty",
				5 => "Bold",
				6 => "Docile",
				7 => "Relaxed",
				8 => "Impish",
				9 => "Lax",
				10 => "Timid",
				11 => "Hasty",
				12 => "Serious",
				13 => "Jolly",
				14 => "Naive",
				15 => "Modest",
				16 => "Mild",
				17 => "Quiet",
				18 => "Bashful",
				19 => "Rash",
				20 => "Calm",
				21 => "Gentle",
				22 => "Sassy",
				23 => "Careful",
				24 => "Quirky",
				_ => "Hardy"
			};
		}

		public static uint[] ParseStats(uint first, uint second)
		{
			uint[] stats = new uint[6];

			stats[0] = first & 0x1F; //HP
			stats[1] = (first & 0x3E0) >> 0x5; //Attack
			stats[2] = (first & 0x7C00) >> 0xA; //Defense

			stats[3] = second & 0x1F; //Speed
			stats[4] = (second & 0x3E0) >> 0x5; //Sp. Attack
			stats[5] = (second & 0x7C00) >> 0xA; //Sp. Defense

			return stats;
		}
	}
}
